using BackEnd.DTO.Response;

namespace BackEnd.Repository
{
    public interface IChartRepository
    {
        Task<int> GetTotalOrdersLast7DaysAsync();
        Task<int> GetTotalOrdersPrevious7DaysAsync();
        Task<decimal> GetTotalRevenueLast7DaysAsync();
        Task<decimal> GetTotalRevenuePrevious7DaysAsync();
        Task<int> GetTotalProductsSoldLast7DaysAsync();
        Task<int> GetTotalProductsSoldPrevious7DaysAsync();
        Task<int> GetNewUsersCountLast7DaysAsync();
        Task<int> GetNewUsersCountPrevious7DaysAsync();
        Task<List<DailyRevenueDto>> GetRevenueLast14DaysAsync();
        Task<List<BestSellingProductDto>> GetBestSellingProductsLast7DaysAsync(int page, int pageSize);
    }
}
