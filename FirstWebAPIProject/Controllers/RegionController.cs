using AutoMapper;
using FirstWebAPIProject.CustomActionFilters;
using FirstWebAPIProject.Data;
using FirstWebAPIProject.Model.Domain;
using FirstWebAPIProject.Model.DTO;
using FirstWebAPIProject.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FirstWebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private IRegionRepository _regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionController> logger;

        public RegionController(IRegionRepository regionRepository,IMapper mapper,ILogger<RegionController> logger)
        {
            _regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAllRegions()
        {
            
                var regionsDomain = await _regionRepository.GetAllRegionAsync();

                return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
        }


        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById(Guid id)
        {

            // Get Region From Database 
            var regionDomain = await _regionRepository.GetRegionByIdAsync(id);

            if (regionDomain == null) return NotFound();

            return Ok(mapper.Map<RegionDTO>(regionDomain));

        }

        [HttpPost]
        [ValidateModel] // Added custom validate model attribute for validations
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddRegion(AddRegionDTO regionDto)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            
                // Covert / Map DTO to Domain Model

                var regionDomain = mapper.Map<Region>(regionDto);

                // Save Domain Model to Database

                regionDomain = await _regionRepository.AddRegionAsync(regionDomain);

                //Covert Domain Model Back to DTO

                var rerurnRegionDto = mapper.Map<RegionDTO>(regionDomain);

                return CreatedAtAction(nameof(GetRegionById), new { id = regionDomain.Id }, rerurnRegionDto);
            
        }

        [HttpPut("{id:guid}")]
        [ValidateModel] // Added custom validate model attribute for validations
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion(Guid id, UpdateRegionDTO updateRegionDTO)
        {

            // Convert DTO to Domain Model

            var regionDomain = mapper.Map<Region>(updateRegionDTO);

            var returnRegionDomainModel = await _regionRepository.UpdateRegionAsync(id, regionDomain);
            if (returnRegionDomainModel == null) return NotFound();           

            // Convert Domain Model To DTO

            return Ok(mapper.Map<RegionDTO>(returnRegionDomainModel));
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            var existingRegion = await _regionRepository.DeleteRegionAsync(id);

            if (existingRegion == null) return NotFound($"No Region Found for Id {id}");
                
            return Ok($"{existingRegion.Name} Region is Deleted Successfully");
        }
    }
}
