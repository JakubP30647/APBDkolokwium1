using Microsoft.Build.Framework;

namespace APBDkolokwium1.Models;

public class postDeliveryDTO
{
    
    [Required]public int deliveryId { get; set; }
    [Required]public int customerId { get; set; }
    [Required]public string licenceNumber { get; set; }
    [Required]public List<Product> Products { get; set; }
    
    
}