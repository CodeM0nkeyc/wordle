var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("WordleClient", cfg =>
{
    cfg.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddMemoryCache();
builder.Services.AddWebEncoders();

builder.Services.AddAuthentication(ApiKeyAuthenticationDefaults.Scheme)
    .AddScheme<ApiKeySettings, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationDefaults.Scheme, opts =>
    {
        opts.ApiKey = builder.Configuration.GetValue<string>("ApiKey")!;
    });

builder.Services.AddControllers();

builder.Services.AddSingleton<IWordleStorage, JsonWordleStorage>();

builder.Services.AddScoped<IWordleApiClient, RandomWordApiClient>();

builder.Services.AddHostedService<WordleRefresherService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
