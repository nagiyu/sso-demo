using IdentityServer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()        //This is for dev only scenarios when you don’t have a certificate to use.
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients);

var app = builder.Build();

app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();
