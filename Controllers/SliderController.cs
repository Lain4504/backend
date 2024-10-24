using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("/api/slider")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class SliderController : ControllerBase
    {
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSliders()
        {
            try
            {
                var sliders = await _sliderService.GetAllSlidersAsync();
                if (sliders == null)
                {
                    return NotFound();
                }
                return Ok(sliders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSliderById(long id)
        {
            try
            {
                var slider = await _sliderService.GetSliderByIdAsync(id);
                if (slider == null)
                {
                    return NotFound();
                }
                return Ok(slider);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSlider([FromBody] Slider slider)
        {
            if (slider == null)
            {
                return BadRequest("Slider is null");
            }

            try
            {
                await _sliderService.AddSliderAsync(slider);
                return CreatedAtAction(nameof(GetSliderById), new { id = slider.Id }, slider);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlider(long id, [FromBody] Slider slider)
        {
            if (slider == null || id != slider.Id)
            {
                return BadRequest("Slider is null or ID mismatch");
            }

            try
            {
                var existingSlider = await _sliderService.GetSliderByIdAsync(id);
                if (existingSlider == null)
                {
                    return NotFound();
                }

                await _sliderService.UpdateSliderAsync(slider);
                return Ok(new { message = "Update successful!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlider(long id)
        {
            try
            {
                var existingSlider = await _sliderService.GetSliderByIdAsync(id);
                if (existingSlider == null)
                {
                    return NotFound();
                }

                await _sliderService.DeleteSliderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
