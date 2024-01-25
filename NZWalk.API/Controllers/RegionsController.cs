using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;

namespace NZWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDBContext dbContext;

        public RegionsController(NZWalksDBContext dBContext) 
        {
            this.dbContext = dBContext;
        }
        //Get all regions
        [HttpGet]
        public IActionResult GetAll() 
        {
            var regions = dbContext.Regions.ToList();

            return Ok(regions);
        }

        //Get single regions (get region by ID)
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromRoute] Guid id) 
        {
            //var region = dbContext.Regions.Find(id);

            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(region);
        }
    }
}
