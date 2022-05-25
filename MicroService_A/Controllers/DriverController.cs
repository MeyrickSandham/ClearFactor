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
    public class DriverController : MainController<Driver, DriverRepository>
    {
        public DriverController(IUnitOfWork unitOfWork, ILogger<DriverController> logger) : base(unitOfWork, logger) { }

        [HttpOptions]
        [Route("Driver/{*url}")]
        public ActionResult Options()
        {
            return Ok();
        }

        [HttpGet]
        [Route("Driver")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Driver>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult<IEnumerable<Driver>>> Get()
        {
            return await BaseGet();
        }

        [HttpGet]
        [Route("Driver/{id}")]
        [ProducesResponseType(statusCode: 200, type: typeof(Driver))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult<Driver>> GetById(Guid id)
        {
            return await BaseGetById(id);
        }

        [HttpPost]
        [Route("Driver")]
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult> Post([FromBody] Driver incoming)
        {
            return await BasePost(incoming);
        }

        [HttpPut]
        [Route("Driver")]
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult> Put([FromBody] Driver incoming)
        {
            return await BasePut(incoming);
        }

        [HttpDelete]
        [Route("Driver/{id}")]
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public async Task<ActionResult> Delete(Guid id)
        {
            return await BaseDelete(id);
        }
    }
}