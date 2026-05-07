using HaberPortali.API.Models;
using HaberPortali.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HaberPortali.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(categories);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound(new { message = "Kategori bulunamadı!" });
            return Ok(category);
        }

        //admin
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveAsync();
            return Ok(new { message = "Kategori başarıyla eklendi." });
        }

        //admin
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.Id) return BadRequest();

            _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync();
            return Ok(new { message = "Kategori başarıyla güncellendi." });
        }

        //admin
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveAsync();
            return Ok(new { message = "Kategori silindi." });
        }
    }
}