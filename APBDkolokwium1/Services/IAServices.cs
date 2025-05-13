using APBDkolokwium1.Models;

namespace APBDkolokwium1.Services;

public interface IAServices
{
    Task<aDTO> getA(string visitId);
    Task addNewA(postADTO newVisit);
}