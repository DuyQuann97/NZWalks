using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalk.API.CustomActionFilters;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;
using NZWalk.API.Reponsitories;

namespace NZWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDBContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDBContext dBContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dBContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        //Get all regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from Database - Domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            //map domain model to dtos
            //var regionsdto = new list<regiondto>();
            //foreach (var regiondomain in regionsdomain)
            //{
            //    regionsdto.add(new regiondto()
            //    {
            //        id = regiondomain.id,
            //        name = regiondomain.name,
            //        code = regiondomain.code,
            //        regionimageurl = regiondomain.regionimageurl,
            //    });
            //}


            //AutoMapper for domain model to dtos
            var regionsDto = mapper.Map<List<RegionDTO>>(regionsDomain);
            //Return DTOs
            return Ok(regionsDto);
        }

        //Get single regions (get region by ID)
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            //Get Region Domain model from Database
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain Model to Region DTO

            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }

        //POST to Create New Region
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map/Convert DTO to domain models

            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
            //Use Domain Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
            //Map Domain Model back to DTO
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            //Return Dto
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }

        //PUT to update Region
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map DTO to domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            //Check if id region exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Model to DTO
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
            //Return DTO

            return Ok(regionDto);

        }

        //Delete region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //check if id region exists
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Send region deleted back
            //Map Region Domain Model to DTO
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDto);
        }
    }
}
