using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;

namespace LccWebAPI.Authentication
{
    public class Config
    {
        private const string LccApiScope = "LccApi";
        private const string ClientSecret = "A9445FF7-A793-429C-8B07-10CF8DB7F6F9";

        private const string LccDesktopApplicationClientId = "LccDeskApplication";

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId()
            };
        }
        
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
                //Client for WPF Application
                new Client
                {
                    ClientId = LccDesktopApplicationClientId,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    
                    ClientSecrets =
                    {
                        new Secret(ClientSecret.Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        LccApiScope
                    }
                   
                }
            };
        }
    }
}
