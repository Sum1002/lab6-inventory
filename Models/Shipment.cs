using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public DateTime ShipmentDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total cost must be non-negative")]
        [DataType(DataType.Currency)]
        public decimal TotalCost { get; set; }

        // Navigation properties
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual Warehouse Warehouse { get; set; } = null!;
    }
}
