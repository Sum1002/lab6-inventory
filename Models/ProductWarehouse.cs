using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class ProductWarehouse
    {
        [Key]
        public int ProductWarehouseId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public int Quantity { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Product Product { get; set; } = null!;
        public virtual Warehouse Warehouse { get; set; } = null!;
    }
}
