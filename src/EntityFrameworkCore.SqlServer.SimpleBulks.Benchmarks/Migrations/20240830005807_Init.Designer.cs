﻿// <auto-generated />
using System;
using EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Migrations
{
    [DbContext(typeof(TestDbContext))]
    [Migration("20240830005807_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Database.CompositeKeyRow<int, int>", b =>
                {
                    b.Property<int>("Id1")
                        .HasColumnType("int");

                    b.Property<int>("Id2")
                        .HasColumnType("int");

                    b.Property<int>("Column1")
                        .HasColumnType("int");

                    b.Property<string>("Column2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Column3")
                        .HasColumnType("datetime2");

                    b.HasKey("Id1", "Id2");

                    b.ToTable("CompositeKeyRows");
                });

            modelBuilder.Entity("EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Database.CompositeKeyRowWithSchema<int, int>", b =>
                {
                    b.Property<int>("Id1")
                        .HasColumnType("int");

                    b.Property<int>("Id2")
                        .HasColumnType("int");

                    b.Property<int>("Column1")
                        .HasColumnType("int");

                    b.Property<string>("Column2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Column3")
                        .HasColumnType("datetime2");

                    b.HasKey("Id1", "Id2");

                    b.ToTable("CompositeKeyRows", "test");
                });

            modelBuilder.Entity("EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Database.ConfigurationEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id1")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<DateTimeOffset>("CreatedDateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsSensitive")
                        .HasColumnType("bit");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Key1");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTimeOffset?>("UpdatedDateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ConfigurationEntry");
                });

            modelBuilder.Entity("EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Database.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Database.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Database.SingleKeyRow<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Column1")
                        .HasColumnType("int");

                    b.Property<string>("Column2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Column3")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SingleKeyRows");
                });

            modelBuilder.Entity("EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Database.SingleKeyRowWithSchema<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Column1")
                        .HasColumnType("int");

                    b.Property<string>("Column2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Column3")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SingleKeyRows", "test");
                });

            modelBuilder.Entity("EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Database.Contact", b =>
                {
                    b.HasOne("EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Database.Customer", "Customer")
                        .WithMany("Contacts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("EntityFrameworkCore.SqlServer.SimpleBulks.Benchmarks.Database.Customer", b =>
                {
                    b.Navigation("Contacts");
                });
#pragma warning restore 612, 618
        }
    }
}
