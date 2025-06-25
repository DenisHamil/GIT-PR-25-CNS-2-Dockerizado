using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class BdProa1Context : DbContext
{
    public BdProa1Context()
    {
    }

    public BdProa1Context(DbContextOptions<BdProa1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AdministrationRequest> AdministrationRequests { get; set; }

    public virtual DbSet<Allergy> Allergies { get; set; }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<HabitualConsumptionMedication> HabitualConsumptionMedications { get; set; }

    public virtual DbSet<Laboratory> Laboratories { get; set; }

    public virtual DbSet<MedicalCouncil> MedicalCouncils { get; set; }

    public virtual DbSet<MedicalHistory> MedicalHistories { get; set; }

    public virtual DbSet<MedicalHistoryAllergy> MedicalHistoryAllergies { get; set; }

    public virtual DbSet<MedicalHistoryLaboratory> MedicalHistoryLaboratories { get; set; }

    public virtual DbSet<MedicalSpecialty> MedicalSpecialties { get; set; }

    public virtual DbSet<Medication> Medications { get; set; }

    public virtual DbSet<MedicationCategory> MedicationCategories { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<RequestDiagnosis> RequestDiagnoses { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Vote> Votes { get; set; }

    public virtual DbSet<TreatmentObservation> TreatmentObservations { get; set; }
    public virtual DbSet<RequestMedication> RequestMedications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            //=> optionsBuilder.UseSqlServer("Data Source=SQL9001.site4now.net;Initial Catalog=db_aaf36f_bdproa;User Id=db_aaf36f_bdproa_admin;Password=Passw0rd;");
            => optionsBuilder.UseSqlServer("Server=sqlserver;Database=bdProa;User Id=sa;Password=Denis71463825;TrustServerCertificate=True;");
    //Data Source=SQL9001.site4now.net;Initial Catalog=db_aaf36f_bdproa;User Id=db_aaf36f_bdproa_admin;Password=YOUR_DB_PASSWORD
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //        => optionsBuilder.UseSqlServer("Server=DESKTOP-PM0C58H\\SQLEXPRESS;Database=bdProa1;User Id=sa;Password=Univalle.;Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdministrationRequest>(entity =>
        {
            entity.HasKey(e => e.IdRequest).HasName("PK__Antibiot__33A8519AF29274A9");

            entity.ToTable("AdministrationRequest");

            entity.Property(e => e.IdRequest).HasColumnName("idRequest");
            entity.Property(e => e.IdMedicalHistory).HasColumnName("idMedicalHistory");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.MedicalJustification)
                .IsUnicode(false)
                .HasColumnName("medicalJustification");
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("priority");
            entity.Property(e => e.Response)
                .IsUnicode(false)
                .HasColumnName("response");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("- Pendiente\r\n- Aprobado\r\n- Rechazado\r\n- Terminado")
                .HasColumnName("status");

            entity.Property(e => e.RegisterDate)
                .HasColumnType("datetime")
                .HasColumnName("registerDate");

            // Configurar la relación con MedicalHistory
            entity.HasOne(d => d.IdMedicalHistoryNavigation).WithMany(p => p.AdministrationRequests)
                .HasForeignKey(d => d.IdMedicalHistory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdministrationRequest_MedicalHistory");

            // Configurar la relación con User
            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.AdministrationRequests)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__Antibioti__UserI__48CFD27E");
        });

        modelBuilder.Entity<Allergy>(entity =>
        {
            entity.HasKey(e => e.IdAllergy);

            entity.ToTable("Allergy");

            entity.Property(e => e.IdAllergy).HasColumnName("idAllergy");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasKey(e => e.IdDiagnosis).HasName("PK__Diagnosi__AC14897E726BF16D");

            entity.ToTable("Diagnosis");

            entity.Property(e => e.IdDiagnosis).HasColumnName("idDiagnosis");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<HabitualConsumptionMedication>(entity =>
        {
            entity
                .HasKey(mca => new { mca.IdMedicalHistory, mca.IdMedication });
            entity.ToTable("HabitualConsumptionMedication");

            entity.Property(e => e.Dosage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("dosage");

            entity.Property(e => e.Frecuency)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("frecuency");
            entity.Property(e => e.IdMedicalHistory).HasColumnName("idMedicalHistory");
            entity.Property(e => e.IdMedication).HasColumnName("idMedication");

            entity.Property(e => e.Administration)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("administrationRoute");
        });

        modelBuilder.Entity<RequestMedication>(entity =>
        {
            entity.HasKey(mca => new { mca.IdRequest, mca.IdMedication });

            entity.ToTable("RequestMedication");

            entity.Property(e => e.Dosage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("dosage");

            entity.Property(e => e.Frecuency)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("frecuency");

            entity.Property(e => e.IdRequest).HasColumnName("idRequest");

            entity.Property(e => e.IdMedication).HasColumnName("idMedication");

            entity.Property(e => e.Administration)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("administrationRoute");

            // Configuración de la relación con Medication
            entity.HasOne(m => m.IdMedicationNavigation)
                .WithMany()
                .HasForeignKey(m => m.IdMedication)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Laboratory>(entity =>
        {
            entity.HasKey(e => e.IdLaboratory).HasName("PK__Laborato__8CC33100C3206E10");

            entity.ToTable("Laboratory");

            entity.Property(e => e.IdLaboratory).HasColumnName("idLaboratory");
            entity.Property(e => e.Range)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("range");
            entity.Property(e => e.TestName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("testName");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<MedicalCouncil>(entity =>
        {
            entity.HasKey(e => e.IdMedicalCouncil).HasName("PK__Advice__4C842CE9167D0B4C");

            entity.ToTable("MedicalCouncil");

            entity.Property(e => e.IdMedicalCouncil).HasColumnName("idMedicalCouncil");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MedicalHistory>(entity =>
        {
            entity.HasKey(e => e.IdMedicalHistory);

            entity.ToTable("MedicalHistory");

            entity.Property(e => e.IdMedicalHistory).HasColumnName("idMedicalHistory");
            entity.Property(e => e.Bmi)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("BMI");
            entity.Property(e => e.Bmiclassification)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("BMIClassification");
            entity.Property(e => e.Height)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("height");
            entity.Property(e => e.IdDiagnosis).HasColumnName("idDiagnosis");
            entity.Property(e => e.IdPatient).HasColumnName("idPatient");
            entity.Property(e => e.Weight)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("weight");
            entity.Property(e => e.RegisterDate)
                .HasColumnType("datetime")
                .HasColumnName("registerDate");
            entity.Property(e => e.Archivepdf)
                .HasColumnType("varbinary(MAX)")
                .HasColumnName("archivepdf");
            entity.Property(e => e.Justificative)
                     .HasMaxLength(200)
                     .IsUnicode(false)
                     .HasColumnName("justificative");
            entity.HasOne(d => d.IdDiagnosisNavigation).WithMany(p => p.MedicalHistories)
                .HasForeignKey(d => d.IdDiagnosis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalHistory_Diagnosis");

            entity.HasOne(d => d.IdPatientNavigation).WithMany(p => p.MedicalHistories)
                .HasForeignKey(d => d.IdPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalHistory_Patient");
           

        });

        //modelBuilder.Entity<MedicalHistoryAllergy>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("MedicalHistoryAllergy");

        //    entity.Property(e => e.IdAllergy).HasColumnName("idAllergy");
        //    entity.Property(e => e.IdMedicalHistory).HasColumnName("idMedicalHistory");

        //    entity.HasOne(d => d.IdAllergyNavigation).WithMany()
        //        .HasForeignKey(d => d.IdAllergy)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_MedicalHistoryAllergy_Allergy");

        //    entity.HasOne(d => d.IdMedicalHistoryNavigation).WithMany()
        //        .HasForeignKey(d => d.IdMedicalHistory)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_MedicalHistoryAllergy_MedicalHistory");
        //});

        modelBuilder.Entity<MedicalHistoryAllergy>()
                .ToTable("MedicalHistoryAllergy")
                .HasKey(ma => new { ma.IdMedicalHistory, ma.IdAllergy });

        modelBuilder.Entity<Vote>().ToTable("Vote")
                 .HasKey(e => e.IdVote);

        //modelBuilder.Entity<MedicalHistoryLaboratory>(entity =>
        //{
        //    entity
        //        .HasNoKey()
        //        .ToTable("MedicalHistoryLaboratory");

        //    entity.Property(e => e.IdLaboratory).HasColumnName("idLaboratory");
        //    entity.Property(e => e.IdMedicalHistory).HasColumnName("idMedicalHistory");
        //    entity.Property(e => e.Result)
        //        .HasColumnType("decimal(5, 2)")
        //        .HasColumnName("result");

        //    entity.HasOne(d => d.IdLaboratoryNavigation).WithMany()
        //        .HasForeignKey(d => d.IdLaboratory)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_MedicalHistoryLaboratory_Laboratory");

        //    entity.HasOne(d => d.IdMedicalHistoryNavigation).WithMany()
        //        .HasForeignKey(d => d.IdMedicalHistory)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_MedicalHistoryLaboratory_MedicalHistory");
        //});
        modelBuilder.Entity<MedicalHistoryLaboratory>()
                .ToTable("MedicalHistoryLaboratory")
                .HasKey(ma => new { ma.IdMedicalHistory, ma.IdLaboratory });

        modelBuilder.Entity<MedicalSpecialty>(entity =>
        {
            entity.HasKey(e => e.IdSpecialty).HasName("PK__MedicalS__631D186FB376995C");

            entity.ToTable("MedicalSpecialty");

            entity.Property(e => e.IdSpecialty).HasColumnName("idSpecialty");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(e => e.IdMedication).HasName("PK__Medicati__4E8C73D8D54F0604");

            entity.ToTable("Medication");

            entity.Property(e => e.IdMedication).HasColumnName("idMedication");
            entity.Property(e => e.Administration)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("administration");
            entity.Property(e => e.IdCategory).HasColumnName("idCategory");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Presentation)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("presentation");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Medications)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK__Medicatio__idCat__5BE2A6F2");
        });

        modelBuilder.Entity<MedicationCategory>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PK__Medicati__79D361B6538E79F8");

            entity.ToTable("MedicationCategory");

            entity.Property(e => e.IdCategory).HasColumnName("idCategory");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.IdPatient).HasName("PK__Patient__970EC34642958EAB");

            entity.ToTable("Patient");

            entity.Property(e => e.IdPatient).HasColumnName("idPatient");
            entity.Property(e => e.PatientCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("patientCode");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.Patients)
                .HasForeignKey(d => d.IdPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Patient__PersonI__398D8EEE");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.IdPerson).HasName("PK__Person__BAB33700056AC1ED");

            entity.ToTable("Person");

            entity.Property(e => e.IdPerson).HasColumnName("idPerson");
            entity.Property(e => e.Ci)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CI");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.SecondLastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("secondLastName");
        });

        modelBuilder.Entity<RequestDiagnosis>(entity =>
        {
            entity.HasKey(e => e.RequestDiagnosisId).HasName("PK__RequestD__AC48FC68806D4889");

            entity.ToTable("RequestDiagnosis");

            entity.Property(e => e.RequestDiagnosisId).HasColumnName("RequestDiagnosisID");
            entity.Property(e => e.DiagnosisId).HasColumnName("DiagnosisID");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");
        });

        /*modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__User__3717C98209C8EABA");

            entity.ToTable("User");

            entity.HasIndex(e => e.Usename, "UQ__User__D1DDB56961A92916").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IdMedicalCouncil).HasColumnName("idMedicalCouncil");
            entity.Property(e => e.IdSpecialty).HasColumnName("idSpecialty");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Usename)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("usename");

            entity.HasOne(d => d.IdMedicalCouncilNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdMedicalCouncil)
                .HasConstraintName("FK_User_MedicalCouncil");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__User__idPerson__5DCAEF64");

            entity.HasOne(d => d.IdSpecialtyNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdSpecialty)
                .HasConstraintName("FK__User__idSpecialt__5EBF139D");
        });*/
        modelBuilder.Entity<User>(entity =>

        {

            entity.HasKey(e => e.IdUser).HasName("PK__User__3717C98209C8EABA");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__D1DDB56961A92916").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("idUser");

            entity.Property(e => e.Email)

                .HasMaxLength(255)

                .IsUnicode(false)

                .HasColumnName("email");

            entity.Property(e => e.IdMedicalCouncil).HasColumnName("idMedicalCouncil");

            entity.Property(e => e.IdSpecialty).HasColumnName("idSpecialty");

            entity.Property(e => e.Password)

                .HasMaxLength(255)

                .IsUnicode(false)

                .HasColumnName("password");

            entity.Property(e => e.Username)

                .HasMaxLength(100)

                .IsUnicode(false)

                .HasColumnName("username");

            entity.Property(e => e.Role)

                .HasMaxLength(30)

                .IsUnicode(false)

                .HasColumnName("role");

            entity.HasOne(d => d.IdMedicalCouncilNavigation).WithMany(p => p.Users)

                .HasForeignKey(d => d.IdMedicalCouncil)

                .HasConstraintName("FK_User_MedicalCouncil");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.Users)

                .HasForeignKey(d => d.IdUser)

                .HasConstraintName("FK__User__idPerson__5DCAEF64");

            entity.HasOne(d => d.IdSpecialtyNavigation).WithMany(p => p.Users)

                .HasForeignKey(d => d.IdSpecialty)

                .HasConstraintName("FK__User__idSpecialt__5EBF139D");

        });

        modelBuilder.Entity<TreatmentObservation>(entity =>
        {
            entity.HasKey(e => e.IdTO).HasName("PK_TreatmentObservation");

            entity.ToTable("TreatmentObservation");

            entity.Property(e => e.IdTO)
                .HasColumnName("idTO")
                .ValueGeneratedNever(); // No se generará un nuevo valor, ya que es el mismo ID de MedicalHistory

            entity.Property(e => e.Observation)
                .IsRequired()
                .HasMaxLength(300) // Tamaño máximo de 300
                .IsUnicode(false)   // Para mapear como varchar
                .HasColumnName("observation");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasColumnName("status"); // Se deja como int

            entity.Property(e => e.RegisterDate)
                .IsRequired()
                .HasColumnType("datetime")
                .HasColumnName("registerDate");

            // Configurar la relación 1 a 1 con MedicalHistory
            entity.HasOne(d => d.MedicalHistoryNavigation)
                .WithOne(p => p.TreatmentObservation)
                .HasForeignKey<TreatmentObservation>(d => d.IdTO) // Usa el mismo IdTO que el de MedicalHistory
                .HasConstraintName("FK_TreatmentObservation_MedicalHistory");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
