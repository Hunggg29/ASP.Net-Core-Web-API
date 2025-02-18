using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Runtime.InteropServices;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        
        // CREATE Walk
        // POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {

                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);

                return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // GET Walks
        // Get: /api/walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walkDomainModel = await walkRepository.GetAllAsync();

            return Ok(mapper.Map<List<WalkDto>>(walkDomainModel));
        }

        // GET Walk By Id
        // Get: /api/walks/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            
            if(walkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // Update Walk By Id
        // PUT: /api/Walks/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
           
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            
            
        }

        // Delete Walk By Id
        // DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
