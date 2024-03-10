using BaseLibrary.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Reposaitories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationsTypeController(IGenericReposaitory<VacationType> _reposaitory) :
        GenericController<VacationType>(_reposaitory)
    {
    }
}
