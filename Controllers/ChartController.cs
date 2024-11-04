using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using BackEnd.DTO.Response;
using Microsoft.AspNetCore.Cors;
using BackEnd.Service;

namespace BackEnd.Controllers
{
    namespace BackEnd.Controllers
    {
        [Route("api/chart")]
        [ApiController]
        [EnableCors("AllowSpecificOrigins")]
        public class ChartController : ControllerBase
        {
            private readonly IChartService _chartService;

            public ChartController(IChartService chartService)
            {
                _chartService = chartService;
            }

            [HttpGet("total-orders-last-7-days")]
            public async Task<IActionResult> GetTotalOrdersLast7Days()
            {
                var result = await _chartService.GetTotalOrdersLast7DaysAsync();
                return Ok(result);
            }

            [HttpGet("total-revenue-last-7-days")]
            public async Task<IActionResult> GetTotalRevenueLast7Days()
            {
                var result = await _chartService.GetTotalRevenueLast7DaysAsync();
                return Ok(result);
            }

            [HttpGet("total-products-sold-last-7-days")]
            public async Task<IActionResult> GetTotalProductsSoldLast7Days()
            {
                var result = await _chartService.GetTotalProductsSoldLast7DaysAsync();
                return Ok(result);
            }

            [HttpGet("new-users-count-last-7-days")]
            public async Task<IActionResult> GetNewUsersCountLast7Days()
            {
                var result = await _chartService.GetNewUsersCountLast7DaysAsync();
                return Ok(result);
            }

            [HttpGet("revenue-last-14-days")]
            public async Task<IActionResult> GetRevenueLast14Days()
            {
                var result = await _chartService.GetRevenueLast14DaysAsync();
                return Ok(result);
            }

            [HttpGet("best-selling-products-last-7-days")]
            public async Task<IActionResult> GetBestSellingProductsLast7Days(int page = 1, int pageSize = 10)
            {
                var result = await _chartService.GetBestSellingProductsLast7DaysAsync(page, pageSize);
                return Ok(result);
            }

        }
    }
}
