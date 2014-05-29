using FlightDataService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArPlaneTrackerClient
{
    public interface IFlightPositionClient
    {
        Task<IList<FlightInfoDTO>> GetData();
    }
}