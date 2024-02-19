using BaseLibrary.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Helpers
{
    public class CustomAuthenticationSatateProviders(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsPrincipal());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stringToken = await localStorageService.GetToken();
            if (string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));

            var deserializeToken = Serializations.DesrializeJsonString<UserSession>(stringToken);
            if (deserializeToken == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var getUserCliams = DecryptToken(deserializeToken.Token!);
            if (getUserCliams == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var CliamsPrincipals = SetCliamsPrincipals(getUserCliams);

             return await Task.FromResult(new AuthenticationState(CliamsPrincipals));
        }

        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            var claimsPrincipals = new ClaimsPrincipal();
            if(userSession.Token != null || userSession.RefreshToken != null)
            {
                var serializesession = Serializations.SerializeObj(userSession);
                await localStorageService.SetToken(serializesession);
                var getUserClaims = DecryptToken(userSession.Token!);
                claimsPrincipals = SetCliamsPrincipals(getUserClaims);            
            }
            else
            {
                await localStorageService.RemoveToken();
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipals)));
        }


       

        private static CustomUserCliams DecryptToken(string JwtToken)
        {
            if(string.IsNullOrEmpty(JwtToken)) return new CustomUserCliams();
            var handler = new JwtSecurityTokenHandler();
            var Token = handler.ReadJwtToken(JwtToken);

            var UserId = Token.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
            var name = Token.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Name);
            var email = Token.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email);
            var role = Token.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Role);

            return new CustomUserCliams(UserId!.Value, name!.Value, email!.Value, role!.Value);
        }

        public static ClaimsPrincipal SetCliamsPrincipals(CustomUserCliams cliams)
        {
            if (cliams.Email is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier,cliams.Id),
                    new(ClaimTypes.Name,cliams.Name),
                    new(ClaimTypes.Email,cliams.Email),
                    new(ClaimTypes.Role,cliams.Role)                            
                },
                "JwtAuth"));
        }
    }
}
