using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductWarehouse> ProductWarehouses { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SKU).HasMaxLength(20);
                
                // Ensure SKU is unique if provided
                entity.HasIndex(e => e.SKU).IsUnique();
            });

            // Configure Warehouse entity
            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.WarehouseId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Location).IsRequired();
                entity.Property(e => e.StorageCapacity).IsRequired();
            });

            // Configure Supplier entity
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierId);
                entity.Property(e => e.CompanyName).IsRequired();
                entity.Property(e => e.ContactEmail).IsRequired();
                
                // Ensure email is unique
                entity.HasIndex(e => e.ContactEmail).IsUnique();
            });

            // Configure ProductWarehouse junction table
            modelBuilder.Entity<ProductWarehouse>(entity =>
            {
                entity.HasKey(e => e.ProductWarehouseId);
                
                // Configure many-to-many relationship between Product and Warehouse
                entity.HasOne(e => e.Product)
                    .WithMany(e => e.ProductWarehouses)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Warehouse)
                    .WithMany(e => e.ProductWarehouses)
                    .HasForeignKey(e => e.WarehouseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.LastUpdated).IsRequired();

                // Ensure unique combination of Product and Warehouse
                entity.HasIndex(e => new { e.ProductId, e.WarehouseId }).IsUnique();
            });

            // Configure Shipment entity
            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.HasKey(e => e.ShipmentId);
                
                // Configure relationship with Supplier
                entity.HasOne(e => e.Supplier)
                    .WithMany(e => e.Shipments)
                    .HasForeignKey(e => e.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with Product
                entity.HasOne(e => e.Product)
                    .WithMany(e => e.Shipments)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with Warehouse
                entity.HasOne(e => e.Warehouse)
                    .WithMany(e => e.Shipments)
                    .HasForeignKey(e => e.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.ShipmentDate).IsRequired();
                entity.Property(e => e.TotalCost).HasColumnType("decimal(18,2)").IsRequired();
            });

            // Seed some initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Laptop", Description = "High-performance laptop", UnitPrice = 999.99m, Category = "Electronics", SKU = "LAP123" },
                new Product { ProductId = 2, Name = "Smartphone", Description = "Latest smartphone model", UnitPrice = 699.99m, Category = "Electronics", SKU = "PHN456" },
                new Product { ProductId = 3, Name = "Desk Chair", Description = "Ergonomic office chair", UnitPrice = 299.99m, Category = "Furniture", SKU = "CHR789" }
            );

            // Seed Warehouses
            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse { WarehouseId = 1, Name = "Main Warehouse", Location = "123 Industrial Rd, City", StorageCapacity = 10000 },
                new Warehouse { WarehouseId = 2, Name = "East Coast Hub", Location = "456 Distribution Ave, City", StorageCapacity = 8000 },
                new Warehouse { WarehouseId = 3, Name = "West Coast Hub", Location = "789 Logistics Blvd, City", StorageCapacity = 12000 }
            );

            // Seed Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { SupplierId = 1, CompanyName = "Tech Supplies Inc.", ContactEmail = "contact@techsupplies.com", ContactPhone = "+1-555-123-4567", Address = "456 Supplier St, City" },
                new Supplier { SupplierId = 2, CompanyName = "Office Furniture Co.", ContactEmail = "orders@officefurniture.com", ContactPhone = "+1-555-987-6543", Address = "789 Furniture Ave, City" },
                new Supplier { SupplierId = 3, CompanyName = "Global Electronics", ContactEmail = "sales@globalelectronics.com", ContactPhone = "+1-555-456-7890", Address = "321 Tech Way, City" }
            );

            // Seed ProductWarehouses
            modelBuilder.Entity<ProductWarehouse>().HasData(
                new ProductWarehouse { ProductWarehouseId = 1, ProductId = 1, WarehouseId = 1, Quantity = 50, LastUpdated = DateTime.Now.AddDays(-1) },
                new ProductWarehouse { ProductWarehouseId = 2, ProductId = 1, WarehouseId = 2, Quantity = 30, LastUpdated = DateTime.Now.AddDays(-2) },
                new ProductWarehouse { ProductWarehouseId = 3, ProductId = 2, WarehouseId = 1, Quantity = 75, LastUpdated = DateTime.Now.AddDays(-1) },
                new ProductWarehouse { ProductWarehouseId = 4, ProductId = 2, WarehouseId = 3, Quantity = 45, LastUpdated = DateTime.Now.AddDays(-3) },
                new ProductWarehouse { ProductWarehouseId = 5, ProductId = 3, WarehouseId = 2, Quantity = 25, LastUpdated = DateTime.Now.AddDays(-1) }
            );

            // Seed Shipments
            modelBuilder.Entity<Shipment>().HasData(
                new Shipment { ShipmentId = 1, SupplierId = 1, ProductId = 1, WarehouseId = 1, Quantity = 20, ShipmentDate = DateTime.Now.AddDays(-5), TotalCost = 19999.80m },
                new Shipment { ShipmentId = 2, SupplierId = 3, ProductId = 2, WarehouseId = 3, Quantity = 15, ShipmentDate = DateTime.Now.AddDays(-3), TotalCost = 10499.85m },
                new Shipment { ShipmentId = 3, SupplierId = 2, ProductId = 3, WarehouseId = 2, Quantity = 10, ShipmentDate = DateTime.Now.AddDays(-1), TotalCost = 2999.90m }
            );
        }
    }
}
