using BaseLibrary.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Reposaitories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralDepartmentController(IGenericReposaitory<GeneralDepartment> _reposaitory) : 
        GenericController<GeneralDepartment>(_reposaitory)
    {

    }
    
}
