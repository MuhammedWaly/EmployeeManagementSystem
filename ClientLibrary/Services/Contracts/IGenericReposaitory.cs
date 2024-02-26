using BaseLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Services.Contracts
{
    public interface IGenericReposaitory<T>
    {
        Task<List<T>> GetAll(string baseUrl);
        Task<T> GetById(int Id, string baseUrl);
        Task<GeneralResponse> Insert(T item, string baseUrl);
        Task<GeneralResponse> Update(T item, string baseUrl);
        Task<GeneralResponse> Delete(int Id, string baseUrl);

    }
}
