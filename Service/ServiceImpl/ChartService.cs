using BackEnd.DTO.Response;
using BackEnd.Repository;

namespace BackEnd.Service.ServiceImpl
{
    public class ChartService : IChartService
    {
        private readonly IChartRepository _chartRepository;

        public ChartService(IChartRepository chartRepository)
        {
            _chartRepository = chartRepository;
        }

        public async Task<object> GetTotalOrdersLast7DaysAsync()
        {
            var totalOrders = await _chartRepository.GetTotalOrdersLast7DaysAsync();
            var previousTotalOrders = await _chartRepository.GetTotalOrdersPrevious7DaysAsync();

            var percentageChange = previousTotalOrders > 0
                ? (double)(totalOrders - previousTotalOrders) / previousTotalOrders * 100
                : 0;

            return new
            {
                totalOrders,
                percentageChange
            };
        }

        public async Task<object> GetTotalRevenueLast7DaysAsync()
        {
            var totalRevenue = await _chartRepository.GetTotalRevenueLast7DaysAsync();
            var previousTotalRevenue = await _chartRepository.GetTotalRevenuePrevious7DaysAsync();

            // Cast to double for percentage change calculation
            var percentageChange = previousTotalRevenue > 0
                ? ((double)(totalRevenue - previousTotalRevenue) / (double)previousTotalRevenue) * 100
                : 0;

            return new
            {
                totalRevenue,
                percentageChange
            };
        }

        public async Task<object> GetTotalProductsSoldLast7DaysAsync()
        {
            var totalProductsSold = await _chartRepository.GetTotalProductsSoldLast7DaysAsync();
            var previousTotalProductsSold = await _chartRepository.GetTotalProductsSoldPrevious7DaysAsync();

            var percentageChange = previousTotalProductsSold > 0
                ? (double)(totalProductsSold - previousTotalProductsSold) / previousTotalProductsSold * 100
                : 0;

            return new
            {
                totalProductsSold,
                percentageChange
            };
        }

        public async Task<object> GetNewUsersCountLast7DaysAsync()
        {
            var newUsersCount = await _chartRepository.GetNewUsersCountLast7DaysAsync();
            var previousNewUsersCount = await _chartRepository.GetNewUsersCountPrevious7DaysAsync();

            var percentageChange = previousNewUsersCount > 0
                ? (double)(newUsersCount - previousNewUsersCount) / previousNewUsersCount * 100
                : 0;

            return new
            {
                newUsersCount,
                percentageChange
            };
        }

        public async Task<List<DailyRevenueDto>> GetRevenueLast14DaysAsync()
        {
            return await _chartRepository.GetRevenueLast14DaysAsync();
        }

        public async Task<object> GetBestSellingProductsLast7DaysAsync(int page, int pageSize)
        {
            var bestSellingProducts = await _chartRepository.GetBestSellingProductsLast7DaysAsync(page, pageSize);
            var totalItems = await _chartRepository.GetBestSellingProductsLast7DaysAsync(1, int.MaxValue); // To get total count

            return new
            {
                Total = totalItems.Count,
                Items = bestSellingProducts
            };
        }
    }
}
