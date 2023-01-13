﻿using Microsoft.AspNetCore.Authorization;

namespace CoivoitEco.API.Handler
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {

            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            var scopes = new List<string>();
            scopes.Add("openid");
            scopes.Add("profile");
            scopes.Add("read:messages");

            var permissions = context.User.FindAll(c => c.Type == "permissions");
            var result = string.Join(",", permissions);
            if (scopes.Any(s => s == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
