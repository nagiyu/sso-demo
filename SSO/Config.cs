using IdentityServer4.Models;

namespace SSO
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "roles",
                    UserClaims = new List<string> { "role" } // ロールを含める
                },
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "client_id",
                    AllowedGrantTypes = GrantTypes.Code, // Authorization Code Flow を許可
                    ClientSecrets =
                    {
                        new Secret("client_secret".Sha256())
                    },
                    RedirectUris =
                    {
                        "http://localhost:5117/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:5117/signout-callback-oidc"
                    },

                    AllowedScopes = { "openid", "profile", "roles", "api1" }, // スコープの設定

                    // リフレッシュトークンの許可
                    AllowOfflineAccess = true,

                    // PKCE (Proof Key for Code Exchange) を有効にする
                    RequirePkce = true,

                    // ロールのクレームを含める
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
    }
}
