using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [Range(0.01, 5000.00, ErrorMessage = "Unit price must be between 0.01 and 5000.00")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string? Category { get; set; }

        [StringLength(20, ErrorMessage = "SKU cannot exceed 20 characters")]
        public string? SKU { get; set; }

        // Navigation properties
        public virtual ICollection<ProductWarehouse> ProductWarehouses { get; set; } = new List<ProductWarehouse>();
        public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
