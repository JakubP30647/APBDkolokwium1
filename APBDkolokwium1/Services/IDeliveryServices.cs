using APBDkolokwium1.Models;

namespace APBDkolokwium1.Services;

public interface IDeliveryServices
{
    Task<DeliveryDTO> getDelivery(string visitId);
    Task addNewDelivery(postDeliveryDTO newVisit);
}