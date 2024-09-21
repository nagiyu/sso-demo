var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add authentication
builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication(options =>
    {
        options.Authority = "https://localhost:5000";
        options.RequireHttpsMetadata = false;
        options.ApiName = "api1";
    });

// Add MVC Core with Authorization and JSON formatters
builder.Services.AddMvcCore()
    .AddAuthorization()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // ここは必要に応じてカスタマイズできます
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// Use Authentication
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
