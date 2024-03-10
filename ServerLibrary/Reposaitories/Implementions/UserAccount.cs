using BaseLibrary.Dtos;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Reposaitories.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Reposaitories.Implementions
{
    public class UserAccount(IOptions<Jwt> config, ApplicationDbContext _context) : IUserAccount
    {


        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            if (user == null) return new GeneralResponse(false, " Model is Empty");

            var CheckUser = await FindUserByEmailAsync(user.Email);
            if (CheckUser != null) return new GeneralResponse(false, "User is already Regitered");

            var applicationUser = await AddToDatabase(new ApplicationUser()
            {
                FullName = user.FullName,
                Email = user.Email,
                password = BCrypt.Net.BCrypt.HashPassword(user.Password)

            });

            var CheckAdmin = await _context.SystemRoles.FirstOrDefaultAsync(R => R.Name!.Equals(Constants.Admin));

            if (CheckAdmin is null)
            {
                var CreateAdminRole = await AddToDatabase(new SystemRole() { Name = Constants.Admin });
                await AddToDatabase(new UserRole() { RoleId = CreateAdminRole.Id, UserId = applicationUser.Id });
                return new GeneralResponse(true, "Account Created");
            }
            var CheckUserRole = await _context.SystemRoles.FirstOrDefaultAsync(R => R.Name!.Equals(Constants.User));
            SystemRole response = new();
            if (CheckUserRole is null)
            {
                var Reponse = await AddToDatabase(new SystemRole() { Name = Constants.User });
                await AddToDatabase(new UserRole() { RoleId = response.Id, UserId = applicationUser.Id });

            }
            else
            {
                await AddToDatabase(new UserRole() { RoleId = CheckUserRole.Id, UserId = applicationUser.Id });
            }

            return new GeneralResponse(true, "Account Created");


        }

        public async Task<LoginResponse> LoginAsync(Login user)
        {
            if (user is null) return new LoginResponse(false, "Model is empty");

            var applicationUser = await FindUserByEmailAsync(user.Email);
            if (applicationUser is null) return new LoginResponse(false, "Email or Password is not valid");

            if (!BCrypt.Net.BCrypt.Verify(user.Password, applicationUser.password)) return new LoginResponse(false, "Email or Password is not valid");

            var GetUserRole = await FindUserRoleAsync(applicationUser.Id);
            if (GetUserRole is null) return new LoginResponse(false, "No user Role");

            var GetRoleName = await FindRoleNameAsync(GetUserRole.RoleId);
            if (GetRoleName is null) return new LoginResponse(false, "No user Role");

            string JwtToken = GenerateToken(applicationUser, GetRoleName.Name);
            string RefreshToken = GenerateRefreshToken();

            var findUser = await _context.RefreshTokenInfos.FirstOrDefaultAsync(u => u.Id == applicationUser.Id);
            if (findUser is not null)
            {
                findUser.Token = RefreshToken;
                await _context.SaveChangesAsync();
            }
            else
            {
                await AddToDatabase(new RefreshTokenInfos() { Token = RefreshToken, UserId = applicationUser.Id });
            }


            return new LoginResponse(true, "Login Successfully", JwtToken, RefreshToken);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private string GenerateToken(ApplicationUser applicationUser, string Rolename)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
            var creditinail = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,applicationUser.Id.ToString()),
                new Claim(ClaimTypes.Name,applicationUser.FullName),
                new Claim(ClaimTypes.Email,applicationUser.Email),
                new Claim(ClaimTypes.Role,Rolename)
            };
            var token = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creditinail
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<T> AddToDatabase<T>(T Model)
        {
            var result = _context.Add(Model);
            await _context.SaveChangesAsync();
            return (T)result.Entity;
        }

        private async Task<ApplicationUser> FindUserByEmailAsync(string email) =>
           await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email!.ToLower()!.Equals(email!.ToLower()));

        private async Task<UserRole> FindUserRoleAsync(int userID) =>
           await _context.UserRoles.FirstOrDefaultAsync(u => u.Id == userID);

        private async Task<SystemRole> FindRoleNameAsync(int RoleID) =>
           await _context.SystemRoles.FirstOrDefaultAsync(u => u.Id == RoleID);


        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken Token)
        {
            if (Token is null) return new LoginResponse(false, "Model is empty");

            var findToken = await _context.RefreshTokenInfos.FirstOrDefaultAsync(t => t.Token!.Equals(Token.Token));
            if (findToken is null) return new LoginResponse(false, "Token is required");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == findToken.UserId);
            if (user == null) return new LoginResponse(false, "No user found");

            var userRole = await FindUserRoleAsync(user.Id);
            var RoleName = await FindRoleNameAsync(userRole.RoleId);
            string JwtToken = GenerateToken(user, RoleName.Name);
            string RefreshToken = GenerateRefreshToken();

            var UpdatedRefreshToken = await _context.RefreshTokenInfos.FirstOrDefaultAsync(u => u.UserId == user.Id);
            if (UpdatedRefreshToken == null) return new LoginResponse(false, "User not Signed in");

            UpdatedRefreshToken.Token = RefreshToken;
            await _context.SaveChangesAsync();
            return new LoginResponse(true, "Token Refreshed Successfully", JwtToken, RefreshToken);
        }



        public async Task<List<ManageUsers>> GetUsers()
        {
            var allUsers = await GetApplicationUsers();
            var allRoles = await GetAllRoles();
            var userRoles = await GetAllUserRoles();
            if (allRoles.Count == 0 || allUsers.Count == 0) return null;

            var users = new List<ManageUsers>();
            foreach (var user in allUsers)
            {
                var userRole = userRoles.FirstOrDefault(u => u.UserId == user.Id);
                var rolename = allRoles.FirstOrDefault(u => u.Id == userRole.RoleId);
                users.Add(new ManageUsers()
                {
                    UserId = user.Id, Name = user.FullName, Role = rolename.Name, Email = user.Email
                });
            }
            return users;
        }

        

        public async Task<GeneralResponse> Update(ManageUsers user)
        {
            var getrole = (await GetAllRoles()).FirstOrDefault(r => r.Name.Equals(user.Role));
            var userrole = await _context.UserRoles.FirstOrDefaultAsync(r => r.UserId == user.UserId);
            userrole.RoleId = getrole.Id;
           await _context.SaveChangesAsync();
            return new GeneralResponse(true, "Role Updated Successfully");
        }

        public async Task<GeneralResponse> Delete(int Id)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == Id);
            _context.ApplicationUsers.Remove(user);
            return new GeneralResponse(true, "User Deleted Successfully");
        }

        public async Task<List<SystemRole>> GetRoles()
        {
           return await GetAllRoles();
        }


        private async Task<List<UserRole>> GetAllUserRoles()
        {
            return await _context.UserRoles.AsNoTracking().ToListAsync();
        }

        private async Task<List<SystemRole>> GetAllRoles()
        {
            return await _context.SystemRoles.AsNoTracking().ToListAsync();
        }

        private async Task<List<ApplicationUser>> GetApplicationUsers()
        {
            return await _context.ApplicationUsers.AsNoTracking().ToListAsync();
        }

    }
}
