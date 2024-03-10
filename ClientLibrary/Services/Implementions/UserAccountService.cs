using BaseLibrary.Dtos;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Services.Implementions
{
    public class UserAccountService(GetHttpClient getHttpClient) : IUserAccountService
    {
        public const string AuthUrl = "api/Authentications";

        public async Task<GeneralResponse> CreateASync(Register user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/Register", user);
            if(!result.IsSuccessStatusCode) return new GeneralResponse(false,"Error occured");

            return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;
        }

        

        public async Task<LoginResponse> LoginASync(Login user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/Login", user);
            if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occured");

            return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken Token)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/Refresh-Token", Token);
            if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occured");

            return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
        }


        public async Task<GeneralResponse> Delete(int Id)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.DeleteAsync($"{AuthUrl}/Delete/{Id}");
            if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Error occured");

            return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;
        }

        public async Task<List<SystemRole>> GetRoles()
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.GetFromJsonAsync<List<SystemRole>>($"{AuthUrl}/Roles");
            return result;
        }

        public async Task<List<ManageUsers>> GetUsers()
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.GetFromJsonAsync<List<ManageUsers>>($"{AuthUrl}/Users");
            return result;
        }


        public async Task<GeneralResponse> Update(ManageUsers user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PutAsJsonAsync($"{AuthUrl}/update", user);
            if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Error occured");

            return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;
        }
    }
}
