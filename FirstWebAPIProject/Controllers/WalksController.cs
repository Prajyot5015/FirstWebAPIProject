using AutoMapper;
using FirstWebAPIProject.CustomActionFilters;
using FirstWebAPIProject.Model.Domain;
using FirstWebAPIProject.Model.DTO;
using FirstWebAPIProject.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalksRepository _walkRepository;

        public WalksController(IMapper mapper,IWalksRepository walksRepository) 
        {
            _mapper = mapper;
            _walkRepository = walksRepository;
        }

        [HttpPost]
        [ValidateModel] // Added custom validate model attribute for validations
        public async Task<IActionResult> AddWalk(AddWalkDTO addWalkDto)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);

            // Map DTo to Domain Model

            var walkDomainModel = _mapper.Map<Walk>(addWalkDto);

            await _walkRepository.AddWalkAsync(walkDomainModel);

            // Map Domain Model To DTO

            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));

        }

        [HttpGet]

        public async Task<IActionResult> GetAllWalks(string? filterOn, string? filterQuery, 
            string? sortBy, bool? isAscending,
            int pageNumber = 1, int pageSize = 1000)
        {
           
                var walkDomainModel = await _walkRepository.GetAllWalksAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
               
            // Covert Walk TO DTO

                return Ok(_mapper.Map<List<WalkDTO>>(walkDomainModel));
           
        }

        [HttpGet("{id:guid}")]

        public async Task<IActionResult> GetWalkById(Guid id)
        {
            var walkDomainModel = await _walkRepository.GetWalkByIdAsync(id);

            if (walkDomainModel == null) return NotFound();

            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));
        }

        [HttpPut("{id:guid}")]
        [ValidateModel] // Added custom validate model attribute for validations
        public async Task<IActionResult> UpdateWalk(Guid id, UpdateWalkDTO updateWalkDto)
        {
            // Covert DTO to Domain Model

            var walkDomainModel = _mapper.Map<Walk>(updateWalkDto);

            walkDomainModel = await _walkRepository.UpdateWalkAsync(id, walkDomainModel);

            if (walkDomainModel == null) return NotFound();          

            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));
        }

        [HttpDelete("{id:guid}")]

        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var result = await _walkRepository.DeleteWalkAsync(id);
            
            if (result == null) return NotFound();

            return Ok(_mapper.Map<WalkDTO>(result));
        }
    }
}
