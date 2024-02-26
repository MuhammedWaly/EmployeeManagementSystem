using BaseLibrary.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers;
using ServerLibrary.Reposaitories.Contracts;


namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController(IGenericReposaitory<Branch> _reposaitory) :
        GenericController<Branch>(_reposaitory)
    {

    }
}
