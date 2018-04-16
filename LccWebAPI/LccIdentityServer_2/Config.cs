using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace LccIdentityServer
{
    public class Config
    {

        // TODO: This can and should be moved to a more secure location and made stronger.
        public static readonly string ClientSecret = "5CD49741-DD56-4B26-8D03-9CF4AAAF9596";

        public static readonly string LccApiScope = "LccApi";

        //Desktop
        public static readonly string LccDesktopApplicationClientId = "ro.LccDesktopApplication";
        public static readonly string LccDesktopApplicationClientName = "Lcc Desktop application";

        //Web
        public static readonly string LccWebClientId = "LccWebApplication";
        public static readonly string LccWebClientName = "Lcc Web Application";

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(LccApiScope, "League Cloud Coach API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // Web login client
                new Client
                {
                    ClientId = LccWebClientId,
                    ClientName = LccWebClientName,
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret(ClientSecret.Sha256())
                    },

                    RedirectUris = {"http://localhost:5002/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:5002/signout-callback-oidc"},

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        LccApiScope
                    },
                    AllowOfflineAccess = true
                },
                //Desktop application login client
                new Client
                {
                    ClientId = LccDesktopApplicationClientId,
                    ClientName = LccDesktopApplicationClientName,

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret(ClientSecret.Sha256())
                    },

                    AllowedScopes =
                    {
                        LccApiScope
                    },
                    
                    AllowOfflineAccess = true
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}
