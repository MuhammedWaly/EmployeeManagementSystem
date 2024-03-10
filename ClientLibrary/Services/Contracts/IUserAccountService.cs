using BaseLibrary.Dtos;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Services.Contracts
{
    public interface IUserAccountService
    {
        Task<GeneralResponse> CreateASync(Register user);
        Task<LoginResponse> LoginASync(Login user);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken Token);

        Task<List<ManageUsers>> GetUsers();
        Task<GeneralResponse> Update(ManageUsers user);
        Task<GeneralResponse> Delete(int Id);
        Task<List<SystemRole>> GetRoles();


    }
}
