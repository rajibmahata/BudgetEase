namespace BudgetEase.Web.Services;

/// <summary>
/// Simple authentication state service for managing user login status.
/// This is a temporary solution until full authentication is implemented.
/// </summary>
public class AuthStateService
{
    private bool _isAuthenticated = false;
    private string? _userName = null;

    public event Action? OnAuthStateChanged;

    public bool IsAuthenticated => _isAuthenticated;
    public string? UserName => _userName;

    public void Login(string userName)
    {
        _isAuthenticated = true;
        _userName = userName;
        NotifyAuthStateChanged();
    }

    public void Logout()
    {
        _isAuthenticated = false;
        _userName = null;
        NotifyAuthStateChanged();
    }

    private void NotifyAuthStateChanged()
    {
        OnAuthStateChanged?.Invoke();
    }
}
