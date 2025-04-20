using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

public partial class MyContext : DbContext
{
    public MyContext()
    {
    }

    public MyContext(DbContextOptions<MyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MeasuringStation> MeasuringStations { get; set; }

    public virtual DbSet<SummaryOfMeasurement> SummaryOfMeasurements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\tzipi\\Desktop\\לימודים\\#c\\הגשת שב\\MeteorologicalService\\ProjectSummary\\station.mdf\";Integrated Security=True;Connect Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MeasuringStation>(entity =>
        {
            entity.HasKey(e => e.StationNumber).HasName("PK__Measurin__26EDF8CC1784FC56");

            entity.ToTable("MeasuringStation");

            entity.Property(e => e.StationNumber).ValueGeneratedNever();
            entity.Property(e => e.StationAddress)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.StationManager)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
            entity.Property(e => e.Town)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnType("text");
        });

        modelBuilder.Entity<SummaryOfMeasurement>(entity =>
        {
            entity.HasKey(e => e.StationNumber).HasName("PK__SummaryO__26EDF8CC09CCD573");

            entity.ToTable("SummaryOfMeasurement");

            entity.Property(e => e.StationNumber).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
