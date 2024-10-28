using BackEnd.DTO.Response;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository.RepositoryImpl
{
    public class ChartRepository : IChartRepository
    {
        private readonly BookStoreContext _context;

        public ChartRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalOrdersLast7DaysAsync()
        {
            return await _context.Orders
                .CountAsync(o => o.Created >= DateTime.Now.AddDays(-7));
        }

        public async Task<int> GetTotalOrdersPrevious7DaysAsync()
        {
            return await _context.Orders
                .CountAsync(o => o.Created >= DateTime.Now.AddDays(-14) && o.Created < DateTime.Now.AddDays(-7));
        }

        public async Task<decimal> GetTotalRevenueLast7DaysAsync()
        {
            return await _context.Orders
                .Where(o => o.Created >= DateTime.Now.AddDays(-7))
                .SumAsync(o => o.TotalPrice) ?? 0;
        }

        public async Task<decimal> GetTotalRevenuePrevious7DaysAsync()
        {
            return await _context.Orders
                .Where(o => o.Created >= DateTime.Now.AddDays(-14) && o.Created < DateTime.Now.AddDays(-7))
                .SumAsync(o => o.TotalPrice) ?? 0;
        }

        public async Task<int> GetTotalProductsSoldLast7DaysAsync()
        {
            return await _context.OrderDetails
                .Where(od => od.Order.Created >= DateTime.Now.AddDays(-7))
                .SumAsync(od => od.Amount) ?? 0;
        }

        public async Task<int> GetTotalProductsSoldPrevious7DaysAsync()
        {
            return await _context.OrderDetails
                .Where(od => od.Order.Created >= DateTime.Now.AddDays(-14) && od.Order.Created < DateTime.Now.AddDays(-7))
                .SumAsync(od => od.Amount) ?? 0;
        }

        public async Task<int> GetNewUsersCountLast7DaysAsync()
        {
            return await _context.Users
                .CountAsync(u => u.Created >= DateTime.Now.AddDays(-7));
        }

        public async Task<int> GetNewUsersCountPrevious7DaysAsync()
        {
            return await _context.Users
                .CountAsync(u => u.Created >= DateTime.Now.AddDays(-14) && u.Created < DateTime.Now.AddDays(-7));
        }

        public async Task<List<DailyRevenueDto>> GetRevenueLast14DaysAsync()
        {
            var today = DateTime.Today;
            var revenueData = new List<DailyRevenueDto>();

            for (int i = 0; i < 14; i++)
            {
                var date = today.AddDays(-i);
                var totalRevenue = await _context.Orders
                    .Where(o => o.Created.HasValue && o.Created.Value.Date == date)
                    .SumAsync(o => o.TotalPrice) ?? 0;

                revenueData.Add(new DailyRevenueDto
                {
                    Date = date,
                    TotalRevenue = totalRevenue
                });
            }

            revenueData.Reverse();
            return revenueData;
        }

        public async Task<List<BestSellingProductDto>> GetBestSellingProductsLast7DaysAsync(int page, int pageSize)
        {
            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            var bestSellingProducts = await _context.OrderDetails
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
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return bestSellingProducts;
        }
    }
}
