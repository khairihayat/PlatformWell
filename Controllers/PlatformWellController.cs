using System.Collections.Generic;
using AutoMapper;
using PlatformWell.Data;
using PlatformWell.Dtos;
using PlatformWell.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace PlatformWell.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlatformWellController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;

        public PlatformWellController(IPlatformRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/platforms
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        //GET api/platforms/{id}
        [HttpGet("{id}", Name="GetPlatformById")]
        public ActionResult <PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);
            if(platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();
        }

        ////POST api/platforms
        //[HttpPost]
        //public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        //{
        //    var platformModel = _mapper.Map<Platform>(platformCreateDto);
        //    _repository.CreatePlatform(platformModel);
        //    _repository.SaveChanges();

        //    var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

        //    return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
        //}

        ////PUT api/platforms/{id}
        //[HttpPut("{id}")]
        //public ActionResult UpdatePlatform(int id, PlatformUpdateDto platformUpdateDto)
        //{
        //    var platformModelFromRepo = _repository.GetPlatformById(id);
        //    if (platformModelFromRepo == null)
        //    {
        //        return NotFound();
        //    }
        //    _mapper.Map(platformUpdateDto, platformModelFromRepo);

        //    _repository.UpdatePlatform(platformModelFromRepo);

        //    _repository.SaveChanges();

        //    return NoContent();
        //}

        ////PATCH api/platforms/{id}
        //[HttpPatch("{id}")]
        //public ActionResult PartialPlatformUpdate(int id, JsonPatchDocument<PlatformUpdateDto> patchDoc)
        //{
        //    var platformModelFromRepo = _repository.GetPlatformById(id);
        //    if (platformModelFromRepo == null)
        //    {
        //        return NotFound();
        //    }

        //    var platformToPatch = _mapper.Map<PlatformUpdateDto>(platformModelFromRepo);
        //    patchDoc.ApplyTo(platformToPatch, ModelState);

        //    if (!TryValidateModel(platformToPatch))
        //    {
        //        return ValidationProblem(ModelState);
        //    }

        //    _mapper.Map(platformToPatch, platformModelFromRepo);

        //    _repository.UpdatePlatform(platformModelFromRepo);

        //    _repository.SaveChanges();

        //    return NoContent();
        //}

        ////DELETE api/platforms/{id}
        //[HttpDelete("{id}")]
        //public ActionResult DeletePlatform(int id)
        //{
        //    var platformModelFromRepo = _repository.GetPlatformById(id);
        //    if (platformModelFromRepo == null)
        //    {
        //        return NotFound();
        //    }
        //    _repository.DeletePlatform(platformModelFromRepo);
        //    _repository.SaveChanges();

        //    return NoContent();
        //}

    }
}