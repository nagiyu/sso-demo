﻿using System.Diagnostics;

using Newtonsoft.Json.Linq;

using IdentityModel.Client;

var client = new HttpClient();

// メタデータからエンドポイントを取得
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
if (disco.IsError)
{
    Debug.WriteLine(disco.Error);
    return;
}

// リソースオーナーパスワードを使用してトークンを要求
var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = "ro.client",
    ClientSecret = "secret",
    UserName = "alice",
    Password = "password",
    Scope = "api1"
});

if (tokenResponse.IsError)
{
    Debug.WriteLine(tokenResponse.Error);
    return;
}

Debug.WriteLine(tokenResponse.Json);

// call api
client.SetBearerToken(tokenResponse.AccessToken);

var response = await client.GetAsync("https://localhost:5001/identity");
if (!response.IsSuccessStatusCode)
{
    Debug.WriteLine(response.StatusCode);
}
else
{
    var content = await response.Content.ReadAsStringAsync();
    Debug.WriteLine(JArray.Parse(content));
}
