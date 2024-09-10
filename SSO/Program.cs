using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSO;
using SSO.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential() // 開発用の署名資格情報（本番環境では実際の証明書を使用）
    .AddInMemoryApiScopes(Config.ApiScopes) // APIスコープの設定
    .AddInMemoryClients(Config.Clients) // クライアントの設定
    .AddInMemoryIdentityResources(Config.IdentityResources) // Identityリソースの設定
    .AddAspNetIdentity<IdentityUser>(); // ASP.NET Core Identityと統合

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseIdentityServer(); // IdentityServerミドルウェアの追加

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
