using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "Warehouse name is required")]
        [StringLength(50, ErrorMessage = "Warehouse name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string Location { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Storage capacity must be non-negative")]
        public int StorageCapacity { get; set; }

        // Navigation properties
        public virtual ICollection<ProductWarehouse> ProductWarehouses { get; set; } = new List<ProductWarehouse>();
        public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
