using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Services.Interfaces;

namespace WushuCompetition.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AgeCategoryController : ControllerBase
    {
        private readonly IAgeCategoryService _ageCategoryService;
        public AgeCategoryController(IAgeCategoryService ageCategoryService)
        {
            _ageCategoryService = ageCategoryService;
        }

        [Authorize]
        [HttpPut("create-age-category")]
        public async Task<ActionResult> CreateAgeCategory(AgeCategoryDto ageCategory)
        {

            try
            {
                await _ageCategoryService.CreateAgeCategory(ageCategory);
                return Ok("success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpGet("get-all-age-categories")]
        public async Task<ActionResult<IEnumerable<AgeCategoryDto>>> GetAgeCategories()
        {
            
            var user = User.Identity.Name;
            try
            {
                var ageCategories = await _ageCategoryService.GetAgeCategories();
                return Ok(ageCategories);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("get-age-categoty-id/{ageCategoryId}")]
        public async Task<ActionResult<AgeCategory>> GetAgeCategory(Guid ageCategoryId)
        {
            try
            {
                var ageCategory = await _ageCategoryService.GetAgeCategoryById(ageCategoryId);
                return Ok(ageCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
