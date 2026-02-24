namespace BudgetEase.Web.Services;

/// <summary>
/// Per-circuit authentication state service for managing user login status and JWT token.
/// Registered as Scoped so each Blazor Server circuit gets its own instance.
/// </summary>
public class AuthStateService
{
    private bool _isAuthenticated;
    private string? _userName;
    private string? _token;
    private string? _userId;
    private string? _email;

    public event Action? OnAuthStateChanged;

    public bool IsAuthenticated => _isAuthenticated;
    public string? UserName => _userName;
    public string? Token => _token;
    public string? UserId => _userId;
    public string? Email => _email;

    public void Login(string userName, string token, string? userId = null, string? email = null)
    {
        _isAuthenticated = true;
        _userName = userName;
        _token = token;
        _userId = userId;
        _email = email;
        NotifyAuthStateChanged();
    }

    public void Logout()
    {
        _isAuthenticated = false;
        _userName = null;
        _token = null;
        _userId = null;
        _email = null;
        NotifyAuthStateChanged();
    }

    private void NotifyAuthStateChanged()
    {
        OnAuthStateChanged?.Invoke();
    }
}
