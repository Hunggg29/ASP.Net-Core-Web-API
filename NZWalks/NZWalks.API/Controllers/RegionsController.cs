﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] //Only the authenticated and authorized users can access the API
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger logger;
        public RegionsController(NZWalksDbContext dbContext,
            IRegionRepository regionRepository, 
            IMapper mapper,
            ILogger<RegionsController> logger) 
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //GET ALL REGIONS
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                throw new Exception("This is a custom exception");

                //Get data (Domain Model) from the database
                var regions = await regionRepository.GetAllAsync();


                //Map the Domain Model to DTOs
                //var regionDtos = new List<RegionDto>(); 
                //foreach(var region in regions)
                //{
                //    regionDtos.Add(new RegionDto()
                //    {
                //        Id = region.Id,
                //        Name = region.Name,
                //        Code = region.Code,
                //        RegionImageUrl = region.RegionImageUrl
                //    });
                //}
                logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regions)}");
                return Ok(mapper.Map<List<RegionDto>>(regions));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
 

           
        }

        //GET SINGLE REGION (Get region by ID)
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id); 
            //"Find" only go with the primary key
            //Get Domain Model from the database using the EF Core
            var region = await regionRepository.GetByIdAsync(id);
            if(region == null)
            {
                return NotFound();
            }
            //Map the Domain Model to DTO

            return Ok(mapper.Map<RegionDto>(region));
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            
                //Map DTOs to Domain Models
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                //Map Domain Model back to Dto
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            
    
            
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            
                //Map updateRegionRequestDto to domain model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //Convert DomainModel to Dto
                return Ok(mapper.Map<RegionDto>(regionDomainModel));
            
       
            

        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert Domain Model to Dto and send it back to the user
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }




    }
}
