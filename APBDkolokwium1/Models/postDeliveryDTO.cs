namespace APBDkolokwium1.Models;

public class postDeliveryDTO
{
    
    public int deliveryId { get; set; }
    public int customerId { get; set; }
    public string licenceNumber { get; set; }
    public List<Product> Products { get; set; }
    
    
}