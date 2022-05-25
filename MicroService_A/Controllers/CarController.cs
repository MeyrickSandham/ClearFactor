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
    public class CarController : MainController<Car, CarRepository>
    {
        public CarController(IUnitOfWork unitOfWork, ILogger<DriverController> logger) : base(unitOfWork, logger) { }

        [HttpOptions]
        [Route("Car/{*url}")]
        public ActionResult Options()
        {
            return Ok();
        }

        [HttpGet]
        [Route("Car")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Car>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult<IEnumerable<Car>>> Get()
        {
            return await BaseGet();
        }

        [HttpGet]
        [Route("Car/{id}")]
        [ProducesResponseType(statusCode: 200, type: typeof(Car))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult<Car>> GetById(Guid id)
        {
            return await BaseGetById(id);
        }

        [HttpPost]
        [Route("Car")]
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult> Post([FromBody] Car incoming)
        {
            return await BasePost(incoming);
        }

        [HttpPost]
        [Route("Car/{carId}/{driverId}")]
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult> AddDriverToCar(Guid carId, Guid driverId)
        {
            Car car = await Repository.GetById(carId);
            car.DriverId = driverId;

            await Repository.Edit(car);

            await Repository.Save();

            return Ok("Successfully added Driver to Car");
        }

        [HttpPut]
        [Route("Car")]
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult> Put([FromBody] Car incoming)
        {
            return await BasePut(incoming);
        }

        [HttpDelete]
        [Route("Car/{id}")]
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult> Delete(Guid id)
        {
            return await BaseDelete(id);
        }
    }
}
