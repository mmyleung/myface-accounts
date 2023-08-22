using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/login")]
    public class LoginController : ControllerBase
    {
        private readonly  IUsersRepo _usersRepo;

        public LoginController(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        [HttpGet("{AuthId}")]
        public IActionResult Authentication(string AuthId)
    {   
        var authHelper = new AuthHelper(AuthId, _usersRepo);
        if (!authHelper.IsAuthenticated)
        {
            return Unauthorized();
        }
        return Ok();
    }

    }
}