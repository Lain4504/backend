using BackEnd.DTO.Response;

namespace BackEnd.Service
{
    public interface IChartService
    {
        Task<object> GetTotalOrdersLast7DaysAsync();
        Task<object> GetTotalRevenueLast7DaysAsync();
        Task<object> GetTotalProductsSoldLast7DaysAsync();
        Task<object> GetNewUsersCountLast7DaysAsync();
        Task<List<DailyRevenueDto>> GetRevenueLast14DaysAsync();
        Task<object> GetBestSellingProductsLast7DaysAsync(int page, int pageSize);
    }
}
