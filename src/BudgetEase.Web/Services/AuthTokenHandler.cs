using System.Net.Http.Headers;

namespace BudgetEase.Web.Services;

/// <summary>
/// Configures HttpClient instances with the current user's JWT bearer token.
/// Used by typed HttpClient services to authenticate API requests.
/// </summary>
public class AuthTokenHandler
{
    private readonly AuthStateService _authState;

    public AuthTokenHandler(AuthStateService authState)
    {
        _authState = authState;
    }

    public void ConfigureClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = null;
        if (!string.IsNullOrEmpty(_authState.Token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _authState.Token);
        }
    }
}
