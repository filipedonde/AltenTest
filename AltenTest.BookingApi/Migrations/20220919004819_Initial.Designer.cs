﻿// <auto-generated />
using System;
using AltenTest.BookingApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AltenTest.BookingApi.Migrations
{
    [DbContext(typeof(BookingContext))]
    [Migration("20220919004819_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AltenTest.BookingApi.Models.Reservation", b =>
                {
                    b.Property<int>("ReservationNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservationNumber"), 2000L, 1);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Guest")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ReservationNumber");

                    b.ToTable("Reservations", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}