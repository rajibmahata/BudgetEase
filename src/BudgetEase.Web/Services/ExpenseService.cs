using BudgetEase.Core.DTOs;
using System.Net.Http.Json;

namespace BudgetEase.Web.Services;

public class ExpenseService
{
    private readonly HttpClient _httpClient;

    public ExpenseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ExpenseDto>> GetExpensesByEventAsync(int eventId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<ExpenseDto>>($"api/expenses/event/{eventId}") 
            ?? new List<ExpenseDto>();
    }

    public async Task<ExpenseDto?> GetExpenseAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ExpenseDto>($"api/expenses/{id}");
    }

    public async Task<ExpenseDto?> CreateExpenseAsync(CreateExpenseDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/expenses", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ExpenseDto>();
    }

    public async Task<ExpenseDto?> UpdateExpenseAsync(int id, UpdateExpenseDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/expenses/{id}", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ExpenseDto>();
    }

    public async Task DeleteExpenseAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/expenses/{id}");
        response.EnsureSuccessStatusCode();
    }
}
