using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using WushuCompetition.Dto;
using WushuCompetition.Models;
using WushuCompetition.Services.Interfaces;

namespace WushuCompetition.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryServices;

        public CategoryController(ICategoryService categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpPut("create-category/{eventId}/{categoryAgeId}")]
        public async Task<ActionResult> AddEvent(CategoryDto categoryDto
            ,Guid eventId, Guid categoryAgeId)
        {
            try
            {
                await _categoryServices.CreateCategory(categoryDto
                    ,eventId,categoryAgeId);

            }
            catch (Exception ex)
            {
                return BadRequest("Something Bad");
            }
            return Ok("Success");
        }

        [HttpGet("get-all-categories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            try
            {
                var categories= await _categoryServices.GetAllCategoriesDto();
                return Ok(categories);
            }
            catch(Exception ex)
            {
                return BadRequest("Something Bad");
            }
            
        }

        [HttpDelete("delete-category")]
        public async Task<ActionResult>DeleteCategory(Guid categoryId)
        {
            try
            {
                await _categoryServices.DeleteCategory(categoryId);
                return Ok("success");
            }
            catch
            {
                return BadRequest("Unable to delete category because there are some participants in this category");
            }
        }

        [HttpGet("get-category/{eventId}")]
        public async Task<ActionResult<IEnumerable<CategoryDataDto>>> GetCategoryData(Guid eventId)
        {
            try
            {
                var categories=await _categoryServices.GetCategoryData(eventId);
                return Ok(categories);
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
    }
}
