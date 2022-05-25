using ClearFactorModelEntity.ControllerDependencies;
using ClearFactorModelEntity.Models;
using ClearFactorModelEntity.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService_A.Controllers
{
    public class TripController : MainController<Trip, TripRepository>
    {
        public TripController(IUnitOfWork unitOfWork, ILogger<TripController> logger) : base(unitOfWork, logger) { }

        [HttpOptions]
        [Route("Trip/{*url}")]
        public ActionResult Options()
        {
            return Ok();
        }

        [HttpGet]
        [Route("Trip")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Trip>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult<IEnumerable<Trip>>> Get()
        {
            return await BaseGet();
        }

        [HttpGet]
        [Route("Trip/{id}")]
        [ProducesResponseType(statusCode: 200, type: typeof(Trip))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult<Trip>> GetById(Guid id)
        {
            return await BaseGetById(id);
        }

        [HttpPost]
        [Route("Trip")]
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult> Post([FromBody] Trip incoming)
        {
            return await BasePost(incoming);
        }

        [HttpPost]
        [Route("Trip/{tripId}/{carId}")]
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult> AddCarToTrip(Guid tripId, Guid carId)
        {
            Trip trip = await Repository.GetById(tripId);
            trip.CarId = carId;

            await Repository.Edit(trip);
            await Repository.Save();

            return Ok("Successfully added Car to Trip");
        }

        [HttpPut]
        [Route("Trip")]
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult> Put([FromBody] Trip incoming)
        {
            return await BasePut(incoming);
        }

        [HttpDelete]
        [Route("Trip/{id}")]
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult> Delete(Guid id)
        {
            return await BaseDelete(id);
        }
    }
}
