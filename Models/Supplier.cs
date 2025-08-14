using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(50, ErrorMessage = "Company name cannot exceed 50 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string ContactEmail { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string? ContactPhone { get; set; }

        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters")]
        public string? Address { get; set; }

        // Navigation properties
        public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
