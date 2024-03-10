using BaseLibrary.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Reposaitories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationsController(IGenericReposaitory<Vacation> _reposaitory) :
        GenericController<Vacation>(_reposaitory)
    {

    }
}
