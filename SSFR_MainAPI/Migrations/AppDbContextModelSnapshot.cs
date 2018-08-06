﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SSFR_MainAPI.Data;
using System;

namespace SSFR_MainAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SSFR_MainAPI.Models.Events", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Date")
                        .IsRequired();

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<string>("Time")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("SSFR_MainAPI.Models.Guest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("EventId");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(1);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("SSFR_MainAPI.Models.HasAssisted", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Assisted");

                    b.Property<int>("EventId");

                    b.Property<int?>("GuestId");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.ToTable("HasAssisted");
                });

            modelBuilder.Entity("SSFR_MainAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Pass")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.Property<string>("ProfUser")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SSFR_MainAPI.Models.WasInvited", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EventId");

                    b.Property<int?>("GuestId");

                    b.Property<bool>("Invited");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.ToTable("WasInvited");
                });

            modelBuilder.Entity("SSFR_MainAPI.Models.HasAssisted", b =>
                {
                    b.HasOne("SSFR_MainAPI.Models.Guest")
                        .WithMany("Assistences")
                        .HasForeignKey("GuestId");
                });

            modelBuilder.Entity("SSFR_MainAPI.Models.WasInvited", b =>
                {
                    b.HasOne("SSFR_MainAPI.Models.Guest")
                        .WithMany("Invitations")
                        .HasForeignKey("GuestId");
                });
#pragma warning restore 612, 618
        }
    }
}