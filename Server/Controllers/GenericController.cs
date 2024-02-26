using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Reposaitories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T> : Controller where T : class
    {
        private readonly IGenericReposaitory<T> _genericReposaitory;

        public GenericController(IGenericReposaitory<T> genericReposaitory)
        {
            _genericReposaitory = genericReposaitory;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll ()
        {
            return Ok(await _genericReposaitory.GetAllAsync());
        }
        
        [HttpGet("GetById{Id}")]
        public async Task<IActionResult> GetById (int Id)
        {
            if (Id <= 0) return BadRequest();
            return Ok(await _genericReposaitory.GetByIdAsync(Id));
        }
        
        [HttpDelete("Delete{Id}")]
        public async Task<IActionResult> Delete (int Id)
        {
            if (Id <= 0) return BadRequest();
            return Ok(await _genericReposaitory.Delete(Id));
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add (T Model)
        {
            if (Model == null) return BadRequest();
            return Ok(await _genericReposaitory.Insert(Model));
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update (T Model)
        {
            if (Model == null) return BadRequest();
            return Ok(await _genericReposaitory.Update(Model));
        }

    }
}
