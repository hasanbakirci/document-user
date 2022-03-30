using System.Net;
using System.Net.Http.Headers;
using core.ServerResponse;
using Newtonsoft.Json;

namespace document_service.Clients.UserClient;

public class UserClient : IUserClient
{
    private readonly HttpClient _httpClient;

    public UserClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Response<TokenHandlerResponse>> TokenValidate(string token)
    {
        var myContent = JsonConvert.SerializeObject(new {token = $"{token}"});
        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(new Uri(UserClientSettings.ValidateTokenUrl), byteContent);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserApiResponse<TokenHandlerResponse>>(responseBody);
            return new SuccessResponse<TokenHandlerResponse>(result.Data);
        }
        throw new UnauthorizedAccessException("Error token");
    }

    public async Task<Response<UserHandlerResponse>> SearchUser(Guid id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(UserClientSettings.SearchUserUrl+id.ToString());
        if (response.StatusCode == HttpStatusCode.OK)
        {
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserApiResponse<UserHandlerResponse>>(responseBody);
            return new SuccessResponse<UserHandlerResponse>(result.Data);
        }

        throw new Exception("id not found");
    }
}
public class UserApiResponse<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
}

public class UserHandlerResponse
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}