using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using UniTransnap.Class;

namespace UniTransnap.Migrations
{
    [DbContext(typeof(HistoryContext))]
    partial class HistoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.3");

            modelBuilder.Entity("UniTransnap.Class.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("after_langage");

                    b.Property<string>("after_word");

                    b.Property<string>("before_langage");

                    b.Property<string>("before_word");

                    b.HasKey("Id");

                    b.ToTable("Historys");
                });
        }
    }
}
