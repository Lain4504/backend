using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using BackEnd.DTO.Response;
using Microsoft.AspNetCore.Cors;

namespace BackEnd.Controllers
{
    [Route("api/chart")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class ChartController : ControllerBase
    {
        private readonly BookStoreContext  _context;

        public ChartController(BookStoreContext context)
        {
            _context = context;
        }

        // API lấy số lượng đơn hàng trong 7 ngày gần đây nhất
        [HttpGet("total-orders-last-7-days")]
        public ActionResult<object> GetTotalOrdersLast7Days()
        {
            var totalOrdersLast7Days = _context.Orders
                .Count(o => o.Created >= DateTime.Now.AddDays(-7));

            var totalOrdersPrevious7Days = _context.Orders
                .Count(o => o.Created >= DateTime.Now.AddDays(-14) && o.Created < DateTime.Now.AddDays(-7));

            var percentageChange = totalOrdersPrevious7Days > 0
                ? (double)(totalOrdersLast7Days - totalOrdersPrevious7Days) / totalOrdersPrevious7Days * 100
                : 0;

            return Ok(new
            {
                totalOrders = totalOrdersLast7Days,
                percentageChange = percentageChange
            });
        }


        // API lấy tổng số tiền thu được trong 7 ngày gần đây nhất
        [HttpGet("total-revenue-last-7-days")]
        public ActionResult<object> GetTotalRevenueLast7Days()
        {
            var totalRevenueLast7Days = _context.Orders
                .Where(o => o.Created >= DateTime.Now.AddDays(-7))
                .Sum(o => o.TotalPrice) ?? 0;

            var totalRevenuePrevious7Days = _context.Orders
                .Where(o => o.Created >= DateTime.Now.AddDays(-14) && o.Created < DateTime.Now.AddDays(-7))
                .Sum(o => o.TotalPrice) ?? 0;

            var percentageChange = totalRevenuePrevious7Days > 0
                ? (double)(totalRevenueLast7Days - totalRevenuePrevious7Days) / totalRevenuePrevious7Days * 100
                : 0;

            return Ok(new
            {
                totalRevenue = totalRevenueLast7Days,
                percentageChange = percentageChange
            });
        }


        // API lấy tổng số lượng sản phẩm đã bán trong 7 ngày gần đây nhất
        [HttpGet("total-products-sold-last-7-days")]
        public ActionResult<object> GetTotalProductsSoldLast7Days()
        {
            var totalProductsSoldLast7Days = _context.OrderDetails
                .Where(od => od.Order.Created >= DateTime.Now.AddDays(-7))
                .Sum(od => od.Amount) ?? 0;

            var totalProductsSoldPrevious7Days = _context.OrderDetails
                .Where(od => od.Order.Created >= DateTime.Now.AddDays(-14) && od.Order.Created < DateTime.Now.AddDays(-7))
                .Sum(od => od.Amount) ?? 0;

            var percentageChange = totalProductsSoldPrevious7Days > 0
                ? (double)(totalProductsSoldLast7Days - totalProductsSoldPrevious7Days) / totalProductsSoldPrevious7Days * 100
                : 0;

            return Ok(new
            {
                totalProductsSold = totalProductsSoldLast7Days,
                percentageChange = percentageChange
            });
        }

        [HttpGet("new-users-count-last-7-days")]
        public ActionResult<object> GetNewUsersCountLast7Days()
        {
            var newUsersLast7Days = _context.Users
                .Where(u => u.Created >= DateTime.Now.AddDays(-7))
                .Count();

            var newUsersPrevious7Days = _context.Users
                .Where(u => u.Created >= DateTime.Now.AddDays(-14) && u.Created < DateTime.Now.AddDays(-7))
                .Count();

            var percentageChange = newUsersPrevious7Days > 0
                ? (double)(newUsersLast7Days - newUsersPrevious7Days) / newUsersPrevious7Days * 100
                : 0;

            return Ok(new
            {
                newUsersCount = newUsersLast7Days,
                percentageChange = percentageChange
            });
        }

        [HttpGet("revenue-last-14-days")]
        public ActionResult<List<DailyRevenueDto>> GetRevenueLast14Days()
        {
            var today = DateTime.Today;
            var revenueData = new List<DailyRevenueDto>();

            for (int i = 0; i < 14; i++)
            {
                var date = today.AddDays(-i);
                var totalRevenue = _context.Orders
                    .Where(o => o.Created.HasValue && o.Created.Value.Date == date)
                    .Sum(o => o.TotalPrice) ?? 0;

                revenueData.Add(new DailyRevenueDto
                {
                    Date = date,
                    TotalRevenue = totalRevenue
                });
            }

            // Đảo ngược danh sách để có thứ tự từ ngày đầu tiên đến ngày cuối cùng
            revenueData.Reverse();

            return Ok(revenueData);
        }
        // API to get best-selling products in the last 7 days
        [HttpGet("best-selling-products-last-7-days")]
        public ActionResult GetBestSellingProductsLast7Days(int page = 1, int pageSize = 10)
        {
            // Set page size to 10, maximum items to 30
            pageSize = Math.Min(pageSize, 10);
            int maxItems = 30;

            var sevenDaysAgo = DateTime.Now.AddDays(-7);

            // Query to calculate total items without pagination
            var bestSellingProductsQuery = _context.OrderDetails
                .Where(od => od.Order.Created >= sevenDaysAgo)
                .GroupBy(od => od.BookId)
                .Select(g => new BestSellingProductDto
                {
                    BookId = g.Key,
                    Title = g.First().Book.Title,
                    TotalOrders = g.Count(),
                    TotalAmountSold = g.Sum(od => od.Amount) ?? 0
                })
                .OrderByDescending(p => p.TotalAmountSold)
                .Take(maxItems); // Limit results to 30 items

            // Total items before pagination
            int totalItems = bestSellingProductsQuery.Count();

            // Apply pagination
            var pagedProducts = bestSellingProductsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return result with total items and paged data
            return Ok(new
            {
                Total = totalItems,
                Items = pagedProducts
            });
        }
    }
}
