using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;

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
            //Get data from Database - Domain models
            var regionsDomain = dbContext.Regions.ToList();

            //Map domain Model to DTOs
            var regionsDto = new List<RegionDTO>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDTO()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl,
                });
            }
            //Return DTOs
            return Ok(regionsDto);
        }

        //Get single regions (get region by ID)
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id) 
        {
            //var region = dbContext.Regions.Find(id);
            //Get Region Domain model from Database
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain Model to Region DTO
            var regionDto = new RegionDTO 
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };
            return Ok(regionDto);
        }

        //POST to Create New Region
        [HttpPost]
        public IActionResult Creat([FromBody] AddRegionRequestDto addRegionRequestDto) 
        {
            //Map/Convert DTO to domain models
            var regionDomainModel = new Region 
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl=addRegionRequestDto.RegionImageUrl,
            };
            //Use Domain Model to create Region
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();
            //Map Domain Model back to DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            //Return Dto
            return CreatedAtAction(nameof(GetById),new {id = regionDto.Id}, regionDto);
        }

        //PUT to update Region
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Check if id region exists
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //Map DTO to Domain Model
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();
            //Map Domain Model to DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            //Return DTO

            return Ok(regionDto);
        }

        //Delete region
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id) 
        {
            //check if id region exists
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();

            //Send region deleted back
            //Map Region Domain Model to DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(regionDto);
        }
    }
}
