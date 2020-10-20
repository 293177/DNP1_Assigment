using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace DNP1_A1.Login
{
    public class Authentication : AuthenticationStateProvider
    {
        private readonly IJSRuntime jsRuntime;
        private readonly IUser userService;
        private User cachedUser;

        public Authentication(IJSRuntime jsRuntime, IUser userService)
        {
            this.jsRuntime = jsRuntime;
            this.userService = userService;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (cachedUser == null)
            {
                string userasJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userasJson))
                {
                    cachedUser = JsonSerializer.Deserialize<User>(userasJson);
                    identity = SetupClaimsForUser(cachedUser);
                }
            }
            else
            {
                identity = SetupClaimsForUser(cachedUser);
            }

            ClaimsPrincipal cashedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cashedClaimsPrincipal));
        }

        public void validateLogin(string username, string password)
        {
            Console.Write("Valid login");
            if (string.IsNullOrEmpty(username)) throw new Exception("Enter username");
            if (string.IsNullOrEmpty(password)) throw new Exception("Enter password");
            ClaimsIdentity identity = new ClaimsIdentity();
            try
            {
                User user = userService.ValidateUser(username, password);
                identity = SetupClaimsForUser(user);
                string serializedData = JsonSerializer.Serialize(user);
                jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serializedData);
                cachedUser = user;
            }
            catch (Exception e)
            {
                throw e;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public void LogOut()
        {
            cachedUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity SetupClaimsForUser(User user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.username));
            claims.Add(new Claim("Role", user.Role));
            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
           return identity;
        }
    }
}