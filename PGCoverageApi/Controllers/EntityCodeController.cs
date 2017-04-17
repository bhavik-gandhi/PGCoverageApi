using System;
using Microsoft.AspNetCore.Mvc;
using PGCoverageApi.Repository;
using PGCoverageApi.Utilities;
using Serilog;
using PGCoverageApi.Models;
using System.Linq;

namespace PGCoverageApi.Controllers
{
    [Route("api/[controller]")]
    public class EntityCodeController : Controller
    {
        private readonly IEntityCodeRepository _entityCodeRepository;
        private ILogger _log;

        public EntityCodeController(ILogger log, IEntityCodeRepository entityCodeRepository)
        {
            _log = log;
            _entityCodeRepository = entityCodeRepository;
        }

        [HttpPatch]
        public IActionResult CreateBulk()
        {
            string connectionString = "Server=bgpostgresmaster.cachftxgju6f.us-east-1.rds.amazonaws.com;User Id=bg;Password=ipreo1359;Database=Orders;Port=5432;Pooling=true;";
            var entityCodes = NSCoverageDataSetup.FetchEntityCode();

            //EntityCode
            var startTimeChannel = DateTime.UtcNow;
            _entityCodeRepository.AddBulk(connectionString, entityCodes);
            
            return Content("Done");

        }
    }
}