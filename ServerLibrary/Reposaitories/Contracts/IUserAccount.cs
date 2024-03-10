using BaseLibrary.Dtos;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Reposaitories.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAsync(Register user);
        Task<LoginResponse> LoginAsync(Login user);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken Token);

        Task<List<ManageUsers>> GetUsers();
        Task<GeneralResponse> Update(ManageUsers user);
        Task<GeneralResponse> Delete(int Id);
        Task<List<SystemRole>> GetRoles();
    }
}
