using costa_serena_grand_hotel_API.AdminModels;
using System.Net.Http.Json;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class AdminApi
    {
        private readonly IHttpClientFactory _factory;

        public AdminApi(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<AdminStatsDto?> GetStatsAsync()
        {
            return await _factory.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<AdminStatsDto>("api/Admin/stats");
        }

        public async Task<List<UserActivityDto>> GetUsersAsync()
        {
            return await _factory.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<UserActivityDto>>("api/Admin/users")
                ?? new List<UserActivityDto>();
        }

        public async Task<UserDetailsDto?> GetUserDetailsAsync(string id)
        {
            return await _factory.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<UserDetailsDto>($"api/Admin/users/{id}");
        }

        public async Task<LogsPagedDto?> GetLogsAsync(
            string? userEmail = null,
            string? entityType = null,
            bool? isAuthFailure = null,
            int page = 1,
            int pageSize = 50)
        {
            var queryParts = new List<string>
            {
                $"page={page}",
                $"pageSize={pageSize}"
            };

            if (!string.IsNullOrWhiteSpace(userEmail))
                queryParts.Add($"userEmail={Uri.EscapeDataString(userEmail)}");

            if (!string.IsNullOrWhiteSpace(entityType))
                queryParts.Add($"entityType={Uri.EscapeDataString(entityType)}");

            if (isAuthFailure.HasValue)
                queryParts.Add($"isAuthFailure={isAuthFailure.Value.ToString().ToLower()}");

            var url = "api/Admin/logs?" + string.Join("&", queryParts);

            return await _factory.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<LogsPagedDto>(url);
        }

        public async Task<List<LogDto>> GetFailedLoginsAsync(int days = 7)
        {
            return await _factory.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<LogDto>>($"api/Admin/logs/failed-logins?days={days}")
                ?? new List<LogDto>();
        }

        public async Task<List<IpStatsDto>> GetIpStatsAsync(int days = 7)
        {
            return await _factory.CreateClient("costa_serena_grand_hotel_API")
                .GetFromJsonAsync<List<IpStatsDto>>($"api/Admin/logs/by-ip?days={days}")
                ?? new List<IpStatsDto>();
        }
    }
}