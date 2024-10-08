﻿using EntityFrameworkCore.SqlServer.SimpleBulks.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EntityFrameworkCore.SqlServer.SimpleBulks.BulkMatch
{
    public class BulkMatchBuilder<T>
    {
        private string _tableName;
        private IEnumerable<string> _matchedColumns;
        private IEnumerable<string> _returnColumns;
        private IDictionary<string, string> _dbColumnMappings;
        private BulkMatchOptions _options;
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public BulkMatchBuilder(SqlConnection connection)
        {
            _connection = connection;
        }

        public BulkMatchBuilder(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public BulkMatchBuilder<T> WithTable(string tableName)
        {
            _tableName = tableName;
            return this;
        }

        public BulkMatchBuilder<T> WithMatchedColumns(string matchedColumn)
        {
            _matchedColumns = new List<string> { matchedColumn };
            return this;
        }

        public BulkMatchBuilder<T> WithMatchedColumns(IEnumerable<string> matchedColumns)
        {
            _matchedColumns = matchedColumns;
            return this;
        }

        public BulkMatchBuilder<T> WithMatchedColumns(Expression<Func<T, object>> matchedColumnsSelector)
        {
            var matchedColumn = matchedColumnsSelector.Body.GetMemberName();
            _matchedColumns = string.IsNullOrEmpty(matchedColumn) ? matchedColumnsSelector.Body.GetMemberNames() : new List<string> { matchedColumn };
            return this;
        }

        public BulkMatchBuilder<T> WithReturnColumns(IEnumerable<string> returnColumns)
        {
            _returnColumns = returnColumns;
            return this;
        }

        public BulkMatchBuilder<T> WithColumns(Expression<Func<T, object>> columnNamesSelector)
        {
            _returnColumns = columnNamesSelector.Body.GetMemberNames().ToArray();
            return this;
        }

        public BulkMatchBuilder<T> WithDbColumnMappings(IDictionary<string, string> dbColumnMappings)
        {
            _dbColumnMappings = dbColumnMappings;
            return this;
        }

        public BulkMatchBuilder<T> ConfigureBulkOptions(Action<BulkMatchOptions> configureOptions)
        {
            _options = new BulkMatchOptions();
            if (configureOptions != null)
            {
                configureOptions(_options);
            }
            return this;
        }

        private string GetDbColumnName(string columnName)
        {
            if (_dbColumnMappings == null)
            {
                return columnName;
            }

            return _dbColumnMappings.ContainsKey(columnName) ? _dbColumnMappings[columnName] : columnName;
        }

        public List<T> Execute(IEnumerable<T> machedValues)
        {
            var temptableName = $"[#{Guid.NewGuid()}]";

            var dataTable = machedValues.ToDataTable(_matchedColumns);
            var sqlCreateTemptable = dataTable.GenerateTableDefinition(temptableName);

            var joinCondition = string.Join(" AND ", _matchedColumns.Select(x =>
            {
                string collation = dataTable.Columns[x].DataType == typeof(string) ?
                $" COLLATE {_options.Collation}" : string.Empty;
                return $"a.[{GetDbColumnName(x)}]{collation} = b.[{x}]{collation}";
            }));

            var selectQueryBuilder = new StringBuilder();
            selectQueryBuilder.AppendLine($"SELECT {string.Join(", ", _returnColumns.Select(x => CreateSelectStatement(x)))} ");
            selectQueryBuilder.AppendLine($"FROM {_tableName} a JOIN {temptableName} b ON " + joinCondition);

            _connection.EnsureOpen();

            Log($"Begin creating temp table:{Environment.NewLine}{sqlCreateTemptable}");

            using (var createTemptableCommand = _connection.CreateTextCommand(_transaction, sqlCreateTemptable, _options))
            {
                createTemptableCommand.ExecuteNonQuery();
            }

            Log("End creating temp table.");


            Log($"Begin executing SqlBulkCopy. TableName: {temptableName}");

            dataTable.SqlBulkCopy(temptableName, null, _connection, _transaction, _options);

            Log("End executing SqlBulkCopy.");

            var selectQuery = selectQueryBuilder.ToString();

            Log($"Begin matching:{Environment.NewLine}{selectQuery}");

            var results = new List<T>();

            var properties = typeof(T).GetProperties().Where(prop => _returnColumns.Contains(prop.Name)).ToList();

            using var updateCommand = _connection.CreateTextCommand(_transaction, selectQuery, _options);
            using var reader = updateCommand.ExecuteReader();
            while (reader.Read())
            {
                T obj = (T)Activator.CreateInstance(typeof(T));

                foreach (var prop in properties)
                {
                    if (!Equals(reader[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, reader[prop.Name], null);
                    }
                }

                results.Add(obj);
            }

            Log($"End matching.");

            return results;
        }

        private string CreateSelectStatement(string colunmName)
        {
            return $"a.[{GetDbColumnName(colunmName)}] as [{colunmName}]";
        }

        private void Log(string message)
        {
            _options?.LogTo?.Invoke($"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff zzz} [BulkMatch]: {message}");
        }
    }
}
