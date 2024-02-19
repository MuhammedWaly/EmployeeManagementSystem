using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Helpers
{
    public class LocalStorageService(ILocalStorageService localStorageService)
    {
        private const string StorageKey = "authentication-Token";
        public async Task<string> GetToken() => await localStorageService.GetItemAsStringAsync(StorageKey);
        public async Task SetToken(string Item) => await localStorageService.SetItemAsStringAsync(StorageKey,Item);
        public async Task RemoveToken() => await localStorageService.RemoveItemAsync(StorageKey);
    }
}
