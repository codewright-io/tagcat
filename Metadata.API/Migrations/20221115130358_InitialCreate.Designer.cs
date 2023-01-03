﻿// <auto-generated />
using CodeWright.Metadata.API.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CodeWright.Metadata.API.Migrations
{
    [DbContext(typeof(MetadataDbContext))]
    [Migration("20221115130358_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("CodeWright.Metadata.API.Queries.Entities.MetadataEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("TenantId")
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id", "TenantId", "Name");

                    b.HasIndex("Id", "TenantId");

                    b.HasIndex("TenantId", "Name", "Value");

                    b.ToTable("Metadata");
                });

            modelBuilder.Entity("CodeWright.Metadata.API.Queries.Entities.ReferenceEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("TenantId")
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TargetId")
                        .HasColumnType("TEXT");

                    b.Property<long>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id", "TenantId", "Type", "TargetId");

                    b.HasIndex("Id", "TenantId");

                    b.ToTable("References");
                });
#pragma warning restore 612, 618
        }
    }
}
