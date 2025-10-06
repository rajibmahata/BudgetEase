using BudgetEase.Web.Components;
using BudgetEase.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure HttpClient for API calls
builder.Services.AddHttpClient<EventService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000");
});
builder.Services.AddHttpClient<VendorService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000");
});
builder.Services.AddHttpClient<ExpenseService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7000");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
