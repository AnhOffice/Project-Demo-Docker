using WebClient.DTOs;

namespace WebClient.Service
{
    public class StudentClientService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7161/api/Students/";
        

        public StudentClientService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ReadDTOs>> GetStudentsAsync()
        {
            //AddAuthorizationHeader(token);
            var url = _baseUrl;
            return await _httpClient
                .GetFromJsonAsync<IEnumerable<ReadDTOs>>(url)
                   ?? Array.Empty<ReadDTOs>();
        }

        public async Task<ReadDTOs> GetStudentByIdAsync(int id)
        {
            //AddAuthorizationHeader(token);
            var url = $"{_baseUrl}{id}";
            var response = await _httpClient.GetAsync(url);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<ReadDTOs>()
                : null;
        }

        public async Task<ReadDTOs?> CreateStudentAsync(CreateDTOs dto)
        {
            //AddAuthorizationHeader(token);
            var url = _baseUrl;
            var response = await _httpClient.PostAsJsonAsync(url, dto);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<ReadDTOs>()
                : null;
        }

        public async Task<bool> UpdateStudentAsync(int id, UpdateDTOs dto)
        {
            //AddAuthorizationHeader(token);
            var url = $"{_baseUrl}{id}";
            var response = await _httpClient.PutAsJsonAsync(url, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            //AddAuthorizationHeader(token);
            var url = $"{_baseUrl}{id}";
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
