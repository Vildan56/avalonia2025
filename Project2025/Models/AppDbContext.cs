using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project2025.Models;


namespace Project2025
{
    public class AppDbContext : DbContext
    {
        public DbSet<Realtor> Realtors { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<HouseRequirement> HouseRequirements { get; set; }
        public DbSet<ApartmentRequirement> ApartmentRequirements { get; set; }
        public DbSet<LandRequirement> LandRequirements { get; set; }
        public DbSet<PropertyObject> PropertyObjects { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Deal> Deals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=DESKTOP-V48DIEU;Database=Immovable2;Trusted_Connection=True;TrustServerCertificate=True;"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация для Requirement -> HouseRequirement (1:1)
            modelBuilder.Entity<HouseRequirement>()
                .HasOne<Requirement>()
                .WithOne()
                .HasForeignKey<HouseRequirement>(hr => hr.RequirementId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HouseRequirement>()
                .HasIndex(hr => hr.RequirementId)
                .IsUnique();

            // Конфигурация для Requirement -> ApartmentRequirement (1:1)
            modelBuilder.Entity<ApartmentRequirement>()
                .HasOne<Requirement>()
                .WithOne()
                .HasForeignKey<ApartmentRequirement>(ar => ar.RequirementId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApartmentRequirement>()
                .HasIndex(ar => ar.RequirementId)
                .IsUnique();

            // Конфигурация для Requirement -> LandRequirement (1:1)
            modelBuilder.Entity<LandRequirement>()
                .HasOne<Requirement>()
                .WithOne()
                .HasForeignKey<LandRequirement>(lr => lr.RequirementId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LandRequirement>()
                .HasIndex(lr => lr.RequirementId)
                .IsUnique();

            // Ограничения для Realtor
            modelBuilder.Entity<Realtor>()
                .HasCheckConstraint("CK_Realtor_CommissionShare", "CommissionShare BETWEEN 0 AND 100");

            // Ограничения для Requirement
            modelBuilder.Entity<Requirement>()
                .HasCheckConstraint("CK_Requirement_MinPrice", "MinPrice >= 0")
                .HasCheckConstraint("CK_Requirement_MaxPrice", "MaxPrice >= 0");

            // Ограничения для HouseRequirement
            modelBuilder.Entity<HouseRequirement>()
                .HasCheckConstraint("CK_HouseRequirement_MinFloors", "MinFloors >= 1")
                .HasCheckConstraint("CK_HouseRequirement_MaxFloors", "MaxFloors >= 1")
                .HasCheckConstraint("CK_HouseRequirement_MinArea", "MinArea >= 0")
                .HasCheckConstraint("CK_HouseRequirement_MaxArea", "MaxArea >= 0")
                .HasCheckConstraint("CK_HouseRequirement_MinRooms", "MinRooms >= 0")
                .HasCheckConstraint("CK_HouseRequirement_MaxRooms", "MaxRooms >= 0");

            // Ограничения для ApartmentRequirement
            modelBuilder.Entity<ApartmentRequirement>()
                .HasCheckConstraint("CK_ApartmentRequirement_MinFloor", "MinFloor >= 1")
                .HasCheckConstraint("CK_ApartmentRequirement_MaxFloor", "MaxFloor >= 1")
                .HasCheckConstraint("CK_ApartmentRequirement_MinArea", "MinArea >= 0")
                .HasCheckConstraint("CK_ApartmentRequirement_MaxArea", "MaxArea >= 0")
                .HasCheckConstraint("CK_ApartmentRequirement_MinRooms", "MinRooms >= 0")
                .HasCheckConstraint("CK_ApartmentRequirement_MaxRooms", "MaxRooms >= 0");

            // Ограничения для LandRequirement
            modelBuilder.Entity<LandRequirement>()
                .HasCheckConstraint("CK_LandRequirement_MinArea", "MinArea >= 0")
                .HasCheckConstraint("CK_LandRequirement_MaxArea", "MaxArea >= 0");

            // Ограничения для Deal
            modelBuilder.Entity<Deal>()
                .HasCheckConstraint("CK_Deal_BuyerRealtorDeduction", "BuyerRealtorDeduction BETWEEN 0 AND 99")
                .HasCheckConstraint("CK_Deal_SellerRealtorDeduction", "SellerRealtorDeduction BETWEEN 0 AND 99");

            // Конфигурация внешних ключей для Requirement
            modelBuilder.Entity<Requirement>()
                .HasOne<Client>()
                .WithMany()
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Requirement>()
                .HasOne<Realtor>()
                .WithMany()
                .HasForeignKey(r => r.RealtorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Requirement>()
                .HasOne<PropertyType>()
                .WithMany()
                .HasForeignKey(r => r.PropertyTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Requirement>()
                .HasOne<District>()
                .WithMany()
                .HasForeignKey(r => r.DistrictId)
                .OnDelete(DeleteBehavior.NoAction);

            // Конфигурация для PropertyObject
            modelBuilder.Entity<PropertyObject>()
                .HasOne<PropertyType>()
                .WithMany()
                .HasForeignKey(po => po.PropertyTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PropertyObject>()
                .HasOne<District>()
                .WithMany()
                .HasForeignKey(po => po.DistrictId)
                .OnDelete(DeleteBehavior.NoAction);

            // Конфигурация для Offer
            modelBuilder.Entity<Offer>()
                .HasOne<Client>()
                .WithMany()
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Offer>()
                .HasOne<Realtor>()
                .WithMany()
                .HasForeignKey(o => o.RealtorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Offer>()
                .HasOne<PropertyObject>()
                .WithMany()
                .HasForeignKey(o => o.PropertyObjectId)
                .OnDelete(DeleteBehavior.NoAction);

            // Конфигурация для Deal
            modelBuilder.Entity<Deal>()
                .HasOne<Requirement>()
                .WithMany()
                .HasForeignKey(d => d.RequirementId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Deal>()
                .HasOne<Offer>()
                .WithMany()
                .HasForeignKey(d => d.OfferId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
