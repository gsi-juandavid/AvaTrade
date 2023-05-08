using AvaTrade.Microservices.DataStorageService.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvaTrade.WebAPI.Controllers
{
    [ApiController]
    [Route("api/news")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService) 
        {
            _newsService = newsService;
        }

        [HttpGet]
        [AllowAnonymous] // TODO: remove this attribute when authentication is implemented
        public IActionResult Get() => Ok(_newsService.GetAsync().Result);        
    }
}
