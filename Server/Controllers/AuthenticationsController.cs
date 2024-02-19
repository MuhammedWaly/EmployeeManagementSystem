using BaseLibrary.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Reposaitories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IUserAccount _userAccount;

        public AuthenticationsController(IUserAccount userAccount)
        {
            _userAccount = userAccount;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> CreateAsync (Register user)
        {
            if (user == null) return BadRequest("Model is not found");
            var result = await _userAccount.CreateAsync(user);
            return Ok(result);
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync (Login user)
        {
            if (user == null) return BadRequest("Model is not found");
            var result = await _userAccount.LoginAsync(user);
            return Ok(result);
        }
        
        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshTokenAsync (RefreshToken Token)
        {
            if (Token == null) return BadRequest("Model is not found");
            var result = await _userAccount.RefreshTokenAsync(Token);
            return Ok(result);
        }
    }
}
