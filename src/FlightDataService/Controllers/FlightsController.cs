using FlightDataService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace FlightDataService.Controllers
{
    public class FlightsController : ApiController
    {
        private IDataHandler _handler;

        public FlightsController(IDataHandler handler)
        {
            _handler = handler;
        }

        public FlightsController()
        {
            _handler = WebApiApplication.DataHandler.Value;
        }

        public async Task<IList<FlightInfoDTO>> Get()
        {
            var infos = await _handler.RequestFlights();
            return infos.MapToDTO();
        }

        [Route("api/flights/{latitude}/{longitude}/{altitude}")]
        [HttpGet]
        public async Task<IList<FlightInfoDTO>> Get(double latitude, double longitude, double altitude)
        {
            var infos = await _handler.RequestFlights(latitude, longitude, altitude);
            return infos.MapToDTO();
        }

        [Route("allflights")]
        [HttpGet]
        public async Task<IList<FlightInfoDTO>> FullList()
        {
            var infos = await _handler.RequestFlights();
            return infos.MapToDTO();
        }
    }
}