using BaseLibrary.Dtos;
using ClientLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Helpers
{
    public class CustomHttpHandler
        (GetHttpClient getHttpClient ,LocalStorageService localStorageService, IUserAccountService userAccountService) : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool loginUrl = request.RequestUri.AbsoluteUri.Contains("Login");
            bool RegisterUrl = request.RequestUri.AbsoluteUri.Contains("Register");
            bool RefreshToken = request.RequestUri.AbsoluteUri.Contains("Refresh-Token");

            if(loginUrl || RegisterUrl || RefreshToken) return await base.SendAsync(request, cancellationToken);
            var result = await base.SendAsync(request, cancellationToken);
            if(result.StatusCode == HttpStatusCode.Unauthorized)
            {
                var stringToken = await localStorageService.GetToken();
                if (stringToken == null) return result;

                string token = string.Empty;
                try
                { 
                    token = request.Headers.Authorization!.Parameter!;
                }
                catch { }

                var desrializedToken = Serializations.DesrializeJsonString<UserSession>(stringToken);
                if (desrializedToken != null) return result;

                if (string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", desrializedToken!.Token);
                    return await base.SendAsync(request, cancellationToken);
                }
                var newJwtToken = await GetRefreshToken(desrializedToken!.RefreshToken!);
                if(string.IsNullOrEmpty(newJwtToken)) return result;

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newJwtToken);
                return await base.SendAsync(request, cancellationToken);
            }
            return result;
        }

        private async Task<string> GetRefreshToken(string refreshToken)
        {
            var result = await userAccountService.RefreshTokenAsync(new RefreshToken() { Token = refreshToken });
            var srializedToken = Serializations.SerializeObj<UserSession>(new UserSession { Token = result.Token, RefreshToken = result.RefreshToken});



            return result.Token;
        }

    }
}
