using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// サービスの設定
builder.Services.AddAuthentication(options =>
{
    // デフォルトの認証スキームを設定
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme) // Cookieでセッションを管理
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        // OAuth 2.0 / OpenID ConnectのプロバイダーURL（認証サーバー）
        options.Authority = "http://localhost:5177"; // 認証サーバーのURLを指定
        options.RequireHttpsMetadata = false; // HTTPS要件を無効にする
        options.ClientId = "client_id"; // クライアントID
        options.ClientSecret = "client_secret"; // クライアントシークレット
        options.ResponseType = OpenIdConnectResponseType.Code; // Authorization Code Flowを使用

        // 認証後のリダイレクトURI
        options.CallbackPath = "/signin-oidc";

        // ログアウト後のリダイレクトURI
        options.SignedOutCallbackPath = "/signout-callback-oidc";

        // スコープの設定
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("api1");
        options.Scope.Add("roles"); // ロール情報を含むスコープを追加

        // トークンを保存する設定
        options.SaveTokens = true;

        // トークンの検証設定
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            NameClaimType = "name", // ユーザー名のクレーム
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" // ロールクレームのタイプを設定
        };

        // 認証後のリダイレクト処理
        options.Events = new OpenIdConnectEvents
        {
            OnRedirectToIdentityProviderForSignOut = (context) =>
            {
                // サインアウト時の処理（任意）
                context.ProtocolMessage.IdTokenHint = context.Request.Cookies["id_token"];
                return Task.CompletedTask;
            }
        };
    });

// MVCやRazorページなどの追加（必要に応じて）
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

// 認証と認可のミドルウェア
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
