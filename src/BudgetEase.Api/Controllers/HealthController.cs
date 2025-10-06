using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetEase.Infrastructure.Data;

namespace BudgetEase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<HealthController> _logger;

    public HealthController(ApplicationDbContext dbContext, ILogger<HealthController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<HealthCheckResponse>> GetHealth()
    {
        var response = new HealthCheckResponse
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Checks = new Dictionary<string, string>()
        };

        try
        {
            // Check database connectivity
            var canConnect = await _dbContext.Database.CanConnectAsync();
            response.Checks["Database"] = canConnect ? "Healthy" : "Unhealthy";
            
            if (!canConnect)
            {
                response.Status = "Unhealthy";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed for database");
            response.Checks["Database"] = "Unhealthy";
            response.Status = "Unhealthy";
        }

        response.Checks["API"] = "Healthy";

        return response.Status == "Healthy" ? Ok(response) : StatusCode(503, response);
    }

    [HttpGet("ready")]
    public async Task<IActionResult> GetReadiness()
    {
        try
        {
            // Check if database is ready
            var canConnect = await _dbContext.Database.CanConnectAsync();
            return canConnect ? Ok(new { status = "Ready" }) : StatusCode(503, new { status = "Not Ready" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Readiness check failed");
            return StatusCode(503, new { status = "Not Ready", error = ex.Message });
        }
    }

    [HttpGet("live")]
    public IActionResult GetLiveness()
    {
        return Ok(new { status = "Live", timestamp = DateTime.UtcNow });
    }
}

public class HealthCheckResponse
{
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Version { get; set; } = string.Empty;
    public Dictionary<string, string> Checks { get; set; } = new();
}
