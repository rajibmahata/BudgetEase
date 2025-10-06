using BudgetEase.Core.DTOs;
using System.Net.Http.Json;

namespace BudgetEase.Web.Services;

public class EventService
{
    private readonly HttpClient _httpClient;

    public EventService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<EventDto>>("api/events") 
            ?? new List<EventDto>();
    }

    public async Task<EventDto?> GetEventAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<EventDto>($"api/events/{id}");
    }

    public async Task<EventDto?> CreateEventAsync(CreateEventDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/events", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<EventDto>();
    }

    public async Task<EventDto?> UpdateEventAsync(int id, UpdateEventDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/events/{id}", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<EventDto>();
    }

    public async Task DeleteEventAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/events/{id}");
        response.EnsureSuccessStatusCode();
    }
}
