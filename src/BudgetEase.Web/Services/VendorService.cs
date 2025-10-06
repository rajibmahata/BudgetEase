using BudgetEase.Core.DTOs;
using System.Net.Http.Json;

namespace BudgetEase.Web.Services;

public class VendorService
{
    private readonly HttpClient _httpClient;

    public VendorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<VendorDto>> GetVendorsByEventAsync(int eventId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<VendorDto>>($"api/vendors/event/{eventId}") 
            ?? new List<VendorDto>();
    }

    public async Task<VendorDto?> GetVendorAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<VendorDto>($"api/vendors/{id}");
    }

    public async Task<VendorDto?> CreateVendorAsync(CreateVendorDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/vendors", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<VendorDto>();
    }

    public async Task<VendorDto?> UpdateVendorAsync(int id, UpdateVendorDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/vendors/{id}", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<VendorDto>();
    }

    public async Task DeleteVendorAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/vendors/{id}");
        response.EnsureSuccessStatusCode();
    }
}
