using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
            var sliders = await _sliderService.GetAllSlidersAsync();
            if (sliders == null)
            {
                return NotFound();
            }
            return Ok(sliders);
        }
    }
}
