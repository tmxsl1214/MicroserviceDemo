﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("AuthServiceApi", "AuthService API"),
                new ApiResource("CommonServiceApi", "CommonService API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "AuthApiClient",
                    ClientName = "AuthApi Client",
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("P@ssw0rd".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "AuthServiceApi" }
                },
                // resource owner password grant client
                new Client
                {
                    ClientId = "CommonApiClient",
                    ClientName = "CommonApi Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                         new Secret("P@ssw0rd".Sha256())
                    },
                    AllowedScopes = { "CommonServiceApi" }
                },
                // OpenID Connect hybrid flow client (MVC)
                new Client
                {
                    ClientId = "HostWebClient",
                    ClientName = "HostWeb Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets =
                    {
                        new Secret("P@ssw0rd".Sha256())
                    },

                    RedirectUris           = { "http://localhost:8600/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:8600/signout-callback-oidc" },

                    RequireConsent = false, //禁用 consent 页面确认

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "CommonServiceApi"
                    }
                }
            };
        }
    }
}