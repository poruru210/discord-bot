using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Models;
using DiscordBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndicatorController : ControllerBase
    {
        private readonly ILogger<IndicatorController> _logger;
        private readonly IndicatorService _service;

        public IndicatorController(ILogger<IndicatorController> logger, IndicatorService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public Task<IEnumerable<EconomicAnnounce>> Get(string currency)
        {
            return _service.hoge();
        }
    }
}
