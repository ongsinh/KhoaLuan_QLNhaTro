﻿// <auto-generated />
using System;
using KhoaLuan_QLNhaTro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KhoaLuan_QLNhaTro.Migrations
{
    [DbContext(typeof(NhaTroDbContext))]
    [Migration("20241125064209_a88")]
    partial class a88
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Asset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Bill", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("UserId");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Contract", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Deposit")
                        .HasColumnType("int");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoomId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.DetailBill", b =>
                {
                    b.Property<string>("BillId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("NewNumber")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<decimal?>("OldNumber")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BillId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("DetailBills");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.DetailContract", b =>
                {
                    b.Property<string>("ContractId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("AssetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("ContractId", "AssetId");

                    b.HasIndex("AssetId");

                    b.ToTable("DetailContracts");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.House", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("floorNumber")
                        .HasColumnType("int");

                    b.Property<int>("roomNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Incident", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Incidents");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.IncidentRoom", b =>
                {
                    b.Property<Guid>("IncidentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IncidentId", "RoomId");

                    b.HasIndex("RoomId");

                    b.ToTable("IncidentRooms");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Acreage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("FLoorNumber")
                        .HasColumnType("int");

                    b.Property<Guid?>("HouseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("HouseId");

                    b.HasIndex("UserId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.RoomService", b =>
                {
                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("RoomId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("RoomsServices");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Account", b =>
                {
                    b.HasOne("KhoaLuan_QLNhaTro.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Asset", b =>
                {
                    b.HasOne("KhoaLuan_QLNhaTro.Models.Room", "Room")
                        .WithMany("Assets")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Bill", b =>
                {
                    b.HasOne("KhoaLuan_QLNhaTro.Models.Room", "Room")
                        .WithMany("Bills")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("KhoaLuan_QLNhaTro.Models.User", "User")
                        .WithMany("Bills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Contract", b =>
                {
                    b.HasOne("KhoaLuan_QLNhaTro.Models.Room", "Room")
                        .WithOne("Contract")
                        .HasForeignKey("KhoaLuan_QLNhaTro.Models.Contract", "RoomId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("KhoaLuan_QLNhaTro.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.DetailBill", b =>
                {
                    b.HasOne("KhoaLuan_QLNhaTro.Models.Bill", "Bill")
                        .WithMany()
                        .HasForeignKey("BillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KhoaLuan_QLNhaTro.Models.Service", "Service")
                        .WithMany("DetailBills")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bill");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.DetailContract", b =>
                {
                    b.HasOne("KhoaLuan_QLNhaTro.Models.Asset", "Asset")
                        .WithMany()
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KhoaLuan_QLNhaTro.Models.Contract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Asset");

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Incident", b =>
                {
                    b.HasOne("KhoaLuan_QLNhaTro.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.IncidentRoom", b =>
                {
                    b.HasOne("KhoaLuan_QLNhaTro.Models.Incident", "Incident")
                        .WithMany()
                        .HasForeignKey("IncidentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KhoaLuan_QLNhaTro.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Incident");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Room", b =>
                {
                    b.HasOne("KhoaLuan_QLNhaTro.Models.House", "House")
                        .WithMany()
                        .HasForeignKey("HouseId");

                    b.HasOne("KhoaLuan_QLNhaTro.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("House");

                    b.Navigation("User");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.RoomService", b =>
                {
                    b.HasOne("KhoaLuan_QLNhaTro.Models.Room", "Room")
                        .WithMany("RoomServices")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KhoaLuan_QLNhaTro.Models.Service", "Service")
                        .WithMany("RoomServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.User", b =>
                {
                    b.HasOne("KhoaLuan_QLNhaTro.Models.Account", "Account")
                        .WithOne("User")
                        .HasForeignKey("KhoaLuan_QLNhaTro.Models.User", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Account", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Room", b =>
                {
                    b.Navigation("Assets");

                    b.Navigation("Bills");

                    b.Navigation("Contract")
                        .IsRequired();

                    b.Navigation("RoomServices");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.Service", b =>
                {
                    b.Navigation("DetailBills");

                    b.Navigation("RoomServices");
                });

            modelBuilder.Entity("KhoaLuan_QLNhaTro.Models.User", b =>
                {
                    b.Navigation("Bills");
                });
#pragma warning restore 612, 618
        }
    }
}
