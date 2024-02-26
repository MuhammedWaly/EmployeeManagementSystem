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
    public class GenericReposaitory<T>(GetHttpClient getHttpClient) : IGenericReposaitory<T>
    {

        public async Task<List<T>> GetAll(string baseUrl)
        {
            var httpClient = await getHttpClient.GetPrivateClient();
           return await httpClient.GetFromJsonAsync<List<T>>($"{baseUrl}/GetAll");
        }

        public async Task<T> GetById(int Id, string baseUrl)
        {
            var httpClient = await getHttpClient.GetPrivateClient();
            var result = await httpClient.GetFromJsonAsync<T>($"{baseUrl}/GetById/{Id}");
            return result!;
        }

        public async Task<GeneralResponse> Insert(T item, string baseUrl)
        {
            var httpClient = await getHttpClient.GetPrivateClient();
            var response = await httpClient.PostAsJsonAsync($"{baseUrl}/add", item);
            var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return result!; 
        }

        public async Task<GeneralResponse> Update(T item, string baseUrl)
        {
            var httpClient = await getHttpClient.GetPrivateClient();
            var response = await httpClient.PutAsJsonAsync($"{baseUrl}/Update", item);
            var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return result!;
        }


        public async Task<GeneralResponse> Delete(int Id, string baseUrl)
        {
            var httpClient = await getHttpClient.GetPrivateClient();
            var response = await httpClient.DeleteAsync($"{baseUrl}/Delete/{Id}");
            var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return result!;
        }

       
    }
}
