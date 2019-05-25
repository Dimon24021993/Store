﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Store.DAL;

namespace Store.DAL.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20190525084606_initial1")]
    partial class initial1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Store.Domain.Entities.Criteria", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("GroupId");

                    b.Property<string>("Name");

                    b.Property<int>("Sort");

                    b.Property<string>("UnitOfMeasure");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Criterias");
                });

            modelBuilder.Entity("Store.Domain.Entities.CriteriaItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CriteriaId");

                    b.Property<Guid?>("ItemId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("CriteriaId");

                    b.HasIndex("ItemId");

                    b.ToTable("CriteriaItems");
                });

            modelBuilder.Entity("Store.Domain.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("GroupType");

                    b.Property<string>("Name");

                    b.Property<Guid?>("ParentId");

                    b.Property<int>("Sort");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Store.Domain.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(12,10)");

                    b.Property<Guid?>("GroupId");

                    b.Property<string>("Name");

                    b.Property<int>("Sort");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Store.Domain.Entities.Picture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("GroupId");

                    b.Property<int>("Height");

                    b.Property<string>("Href");

                    b.Property<Guid?>("ItemId");

                    b.Property<string>("Name");

                    b.Property<int>("Sort");

                    b.Property<int>("SourceType");

                    b.Property<int>("Type");

                    b.Property<int>("Width");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("ItemId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("Store.Domain.Entities.Rate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<Guid>("ItemId");

                    b.Property<int>("RateType");

                    b.Property<Guid?>("UserId");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(12,10)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("Rates");
                });

            modelBuilder.Entity("Store.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("RoleDescription");

                    b.Property<string>("RoleName")
                        .IsRequired();

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Store.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<int>("Language");

                    b.Property<string>("LastName");

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<string>("MiddleName");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Store.Domain.Entities.Criteria", b =>
                {
                    b.HasOne("Store.Domain.Entities.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("Store.Domain.Entities.CriteriaItem", b =>
                {
                    b.HasOne("Store.Domain.Entities.Criteria", "Criteria")
                        .WithMany()
                        .HasForeignKey("CriteriaId");

                    b.HasOne("Store.Domain.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId");
                });

            modelBuilder.Entity("Store.Domain.Entities.Group", b =>
                {
                    b.HasOne("Store.Domain.Entities.Group", "ParentGroup")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Store.Domain.Entities.Item", b =>
                {
                    b.HasOne("Store.Domain.Entities.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("Store.Domain.Entities.Picture", b =>
                {
                    b.HasOne("Store.Domain.Entities.Group", "Group")
                        .WithMany("Pictures")
                        .HasForeignKey("GroupId");

                    b.HasOne("Store.Domain.Entities.Item", "Item")
                        .WithMany("Pictures")
                        .HasForeignKey("ItemId");
                });

            modelBuilder.Entity("Store.Domain.Entities.Rate", b =>
                {
                    b.HasOne("Store.Domain.Entities.Item", "Item")
                        .WithMany("Rated")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Store.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Store.Domain.Entities.Role", b =>
                {
                    b.HasOne("Store.Domain.Entities.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
