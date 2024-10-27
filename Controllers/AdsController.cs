using BackEnd.Models;
using BackEnd.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("/api/ads")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class AdsController : ControllerBase
    {
        private readonly IAdsService _adsService;

        public AdsController(IAdsService adsService)
        {
            _adsService = adsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAds()
        {
            var ads = await _adsService.GetAllAdsAsync();
            if (ads == null || !ads.Any())
            {
                return NotFound();
            }
            return Ok(ads);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdsById(int id)
        {
            var ads = await _adsService.GetAdsByIdAsync(id);
            if (ads == null)
            {
                return NotFound();
            }
            return Ok(ads);
        }

        [HttpPost]
        public async Task<IActionResult> AddAds([FromBody] Ads ads)
        {
            if (ads == null)
            {
                return BadRequest("Ads is null");
            }
            await _adsService.AddAdsAsync(ads);
            return CreatedAtAction(nameof(GetAdsById), new { id = ads.Id }, ads);
        }

        [HttpPut("{id}")]   
        public async Task<IActionResult> UpdateAds(int id, [FromBody] Ads ads)
        {
            if (ads == null || id != ads.Id)
            {
                return BadRequest("Ads is null or ID mismatch");
            }

            var existingAds = await _adsService.GetAdsByIdAsync(id);
            if (existingAds == null)
            {
                return NotFound();
            }

            await _adsService.UpdateAdsAsync(ads);
            return Ok(new { message = "Update successful!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAds(int id)
        {
            var existingAds = await _adsService.GetAdsByIdAsync(id);
            if (existingAds == null)
            {
                return NotFound();
            }

            await _adsService.DeleteAdsAsync(id);
            return NoContent();
        }
    }
}
