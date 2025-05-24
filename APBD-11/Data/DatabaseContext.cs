using APBD_11.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_11.Data;

public class DatabaseContext : DbContext
{
    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<Patient> Patients { get; set; }
    public virtual DbSet<Medicament> Medicaments { get; set; }
    public virtual DbSet<Prescription> Prescriptions { get; set; }
    public virtual DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { IdDoctor = 1, FirstName = "Doctor 1", LastName = "1", Email = "doctor1@ex.com" },
            new Doctor { IdDoctor = 2, FirstName = "Doctor 2", LastName = "2", Email = "doctor2@ex.com" },
            new Doctor { IdDoctor = 3, FirstName = "Doctor 3", LastName = "3", Email = "doctor3@ex.com" },
            new Doctor { IdDoctor = 4, FirstName = "Doctor 4", LastName = "4", Email = "doctor4@ex.com" },
            new Doctor { IdDoctor = 5, FirstName = "Doctor 5", LastName = "5", Email = "doctor5@ex.com" }
        );

        modelBuilder.Entity<Patient>().HasData(
            new Patient { IdPatient = 1, FirstName = "Patient 1", LastName = "1", Birthdate = new DateTime(2000, 1, 1) },
            new Patient { IdPatient = 2, FirstName = "Patient 2", LastName = "2", Birthdate = new DateTime(2001, 1, 1) },
            new Patient { IdPatient = 3, FirstName = "Patient 3", LastName = "3", Birthdate = new DateTime(2002, 1, 1) },
            new Patient { IdPatient = 4, FirstName = "Patient 4", LastName = "4", Birthdate = new DateTime(2003, 1, 1) },
            new Patient { IdPatient = 5, FirstName = "Patient 5", LastName = "5", Birthdate = new DateTime(2004, 1, 1) }
        );

        modelBuilder.Entity<Medicament>().HasData(
            new Medicament { IdMedicament = 1, Name = "Medicament 1", Description = "Very nice", Type = "Type 2" },
            new Medicament { IdMedicament = 2, Name = "Medicament 2", Description = "Awesome", Type = "Type 1" },
            new Medicament { IdMedicament = 3, Name = "Medicament 3", Description = "Perfect medicament", Type = "Type 2" }
        );
    }
}