using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        public RegionsController(NZWalksDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        //GET ALL REGIONS
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get data (Domain Model) from the database
            var regions = dbContext.Regions.ToList();
            //Map the Domain Model to DTOs
            var regionDtos = new List<RegionDto>(); 
            foreach(var region in regions)
            {
                regionDtos.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            return Ok(regionDtos);
        }

        //GET SINGLE REGION (Get region by ID)
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id); 
            //"Find" only go with the primary key
            //Get Domain Model from the database using the EF Core
            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(region == null)
            {
                return NotFound();
            }
            //Map the Domail Model to DTO
            var regionDto = new RegionDto()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionDto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map DTOs to Domain Models
            var regionDomainModel = new Region()
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            //Use Domain Model to create new Region using EF Core
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            //Map Domain Model back to Dto
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }



    }
}
