using BudgetEase.Core.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace BudgetEase.Web.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return result ?? new AuthResponse { Success = false, Message = "Invalid response from server" };
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            try
            {
                var errorResponse = JsonSerializer.Deserialize<AuthResponse>(errorContent, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });
                return errorResponse ?? new AuthResponse { Success = false, Message = "Registration failed" };
            }
            catch
            {
                return new AuthResponse { Success = false, Message = errorContent };
            }
        }
        catch (Exception ex)
        {
            return new AuthResponse { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return result ?? new AuthResponse { Success = false, Message = "Invalid response from server" };
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            try
            {
                var errorResponse = JsonSerializer.Deserialize<AuthResponse>(errorContent, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });
                return errorResponse ?? new AuthResponse { Success = false, Message = "Login failed" };
            }
            catch
            {
                return new AuthResponse { Success = false, Message = errorContent };
            }
        }
        catch (Exception ex)
        {
            return new AuthResponse { Success = false, Message = $"Error: {ex.Message}" };
        }
    }
}
