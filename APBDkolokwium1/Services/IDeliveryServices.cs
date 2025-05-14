using APBDkolokwium1.Models;

namespace APBDkolokwium1.Services;

public interface IDeliveryServices
{
    Task<DeliveryDTO> getA(string visitId);
    Task addNewA(postDeliveryDTO newVisit);
}