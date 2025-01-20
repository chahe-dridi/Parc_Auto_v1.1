using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;

namespace Parc_Auto_v3.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
 
 
        public DbSet<Voiture> Voitures { get; set; }
        public DbSet<Marque> Marques { get; set; }
        public DbSet<Modele> Modeles { get; set; }
        public DbSet<VisiteTechnique> VisiteTechniques { get; set; }
        public DbSet<Vidange> Vidanges { get; set; }
        public DbSet<Sinistre> Sinistres { get; set; }
        public DbSet<Assurance> Assurances { get; set; }
        public DbSet<Vignette> Vignettes { get; set; }
        public DbSet<Demandes> Demandes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


       

            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var utilisateur = new IdentityRole("utilisateur");
            utilisateur.NormalizedName = "utilisateur";

            var agent = new IdentityRole("agent");
            agent.NormalizedName = "agent";

            var superadmin = new IdentityRole("superadmin");
            superadmin.NormalizedName = "superadmin";

            modelBuilder.Entity<IdentityRole>().HasData(admin, utilisateur, agent, superadmin);


            modelBuilder.Entity<Voiture>()
                .HasKey(v => v.Id);

            modelBuilder.Entity<Voiture>()
                .Property(v => v.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Voiture>()
                .HasOne(v => v.Marque)
                .WithMany(m => m.Voitures)
                .HasForeignKey(v => v.MarqueId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Voiture>()
                .HasOne(v => v.Modele)
                .WithMany(m => m.Voitures)
                .HasForeignKey(v => v.ModeleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Voiture>()
                .HasMany(v => v.VisiteTechniques)
                .WithOne(vt => vt.Voiture)
                .HasForeignKey(vt => vt.VoitureId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Voiture>()
                .HasMany(v => v.Vidanges)
                .WithOne(vd => vd.Voiture)
                .HasForeignKey(vd => vd.VoitureId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Voiture>()
                .HasMany(v => v.Sinistres)
                .WithOne(s => s.Voiture)
                .HasForeignKey(s => s.VoitureId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Voiture>()
                .HasMany(v => v.Assurances)
                .WithOne(a => a.Voiture)
                .HasForeignKey(a => a.VoitureId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Voiture>()
                .HasMany(v => v.Vignettes)
                .WithOne(vg => vg.Voiture)
                .HasForeignKey(vg => vg.VoitureId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Voiture>()
                .HasMany(v => v.Demandes)
                .WithOne(d => d.Voiture)
                .HasForeignKey(d => d.VoitureId)
                .OnDelete(DeleteBehavior.Cascade);

            // Modele configuration
            modelBuilder.Entity<Modele>()
                .HasOne(m => m.Marque)
                .WithMany(mar => mar.Modeles)
                .HasForeignKey(m => m.MarqueId)
                .OnDelete(DeleteBehavior.Cascade);

            // Demandes configuration
            modelBuilder.Entity<Demandes>()
                .HasOne(d => d.Voiture)
                .WithMany(v => v.Demandes)
                .HasForeignKey(d => d.VoitureId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Demandes>()
              .HasOne(d => d.User)
              .WithMany(u => u.Demandes)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.Cascade);




        }
 
    }
}
