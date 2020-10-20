using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ProductsService.Controllers
{
    //[Authorize(Policy = "PublicSecure2")]
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Cream", "Shampoo", "Cooking Oil", "Ice Cream"
        };

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }
       
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Summaries;
        }
    }
}
