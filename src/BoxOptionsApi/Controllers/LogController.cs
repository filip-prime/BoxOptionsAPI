using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AzureRepositories;
using BoxOptionsApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BoxOptionsApi.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly ILogRepository _logRepository;

        public LogController(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LogDto logDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _logRepository.InsertAsync(new LogEntity()
            {
                ClientId = logDto.ClientId,
                EventCode = logDto.EventCode,
                Message = logDto.Message,
                PartitionKey = logDto.ClientId
            });

            return Ok();
        }

        [HttpGet]
        public async Task<LogDto[]> Get([FromQuery] string dateFrom, [FromQuery] string dateTo,
            [FromQuery] string clientId)
        {
            const string format = "yyyyMMdd";
            var entities = await _logRepository.GetRange(DateTime.ParseExact(dateFrom, format, CultureInfo.InvariantCulture), DateTime.ParseExact(dateTo, format, CultureInfo.InvariantCulture).AddDays(1), clientId);
            return entities.Select(e => new LogDto()
            {
                ClientId = e.ClientId,
                EventCode = e.EventCode,
                Message = e.Message,
                Timestamp = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
            }).ToArray();
        }
    }
}