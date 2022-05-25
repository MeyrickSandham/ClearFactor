using ClearFactorModelEntity.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace MicroService_C.Controllers
{
    public class PenaltyByDriverController : Controller
    {   
        [HttpGet]
        [Route("PenaltyByDriverId/{driverId}")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<dynamic>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public ActionResult PenaltyByDriverId(Guid driverId)
        {            
            MongoClient mongoClient = new MongoClient("mongodb://127.0.0.1:27017");

            IMongoDatabase db = mongoClient.GetDatabase("ClearFactor");

            IMongoCollection<DriverPenaltycollectionItem> driverPenaltyCollection = db.GetCollection<DriverPenaltycollectionItem>("DriverPenalty");

            FilterDefinitionBuilder<DriverPenaltycollectionItem> filterdef = new FilterDefinitionBuilder<DriverPenaltycollectionItem>();
            FilterDefinition<DriverPenaltycollectionItem> filter = filterdef.In(x => x.DriverId, new[] { driverId.ToString() });

            List<DriverPenaltycollectionItem> driverPenaltyList = driverPenaltyCollection.Find(filter).ToList();

            return Ok(driverPenaltyList);
        }
    }
}
