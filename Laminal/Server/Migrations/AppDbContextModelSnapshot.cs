﻿// <auto-generated />
using System;
using Laminal.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Laminal.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Laminal.Shared.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Laminal.Shared.Models.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerProjectId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerProjectId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("Laminal.Shared.Models.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AssigneeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerProjectId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TaskType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("OwnerProjectId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Laminal.Shared.Models.TaskProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("TaskId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskProperties");
                });

            modelBuilder.Entity("TaskTask", b =>
                {
                    b.Property<int>("PredecessorTasksId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SuccessorTasksId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PredecessorTasksId", "SuccessorTasksId");

                    b.HasIndex("SuccessorTasksId");

                    b.ToTable("TaskTask");
                });

            modelBuilder.Entity("Laminal.Shared.Models.Resource", b =>
                {
                    b.HasOne("Laminal.Shared.Models.Project", "OwnerProject")
                        .WithMany("Resources")
                        .HasForeignKey("OwnerProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OwnerProject");
                });

            modelBuilder.Entity("Laminal.Shared.Models.Task", b =>
                {
                    b.HasOne("Laminal.Shared.Models.Resource", "Assignee")
                        .WithMany()
                        .HasForeignKey("AssigneeId");

                    b.HasOne("Laminal.Shared.Models.Project", "OwnerProject")
                        .WithMany("Tasks")
                        .HasForeignKey("OwnerProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("OwnerProject");
                });

            modelBuilder.Entity("Laminal.Shared.Models.TaskProperty", b =>
                {
                    b.HasOne("Laminal.Shared.Models.Task", null)
                        .WithMany("Properties")
                        .HasForeignKey("TaskId");
                });

            modelBuilder.Entity("TaskTask", b =>
                {
                    b.HasOne("Laminal.Shared.Models.Task", null)
                        .WithMany()
                        .HasForeignKey("PredecessorTasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Laminal.Shared.Models.Task", null)
                        .WithMany()
                        .HasForeignKey("SuccessorTasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Laminal.Shared.Models.Project", b =>
                {
                    b.Navigation("Resources");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Laminal.Shared.Models.Task", b =>
                {
                    b.Navigation("Properties");
                });
#pragma warning restore 612, 618
        }
    }
}
