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
    }
}