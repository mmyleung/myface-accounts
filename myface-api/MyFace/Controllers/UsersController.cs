using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Helpers;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepo _users;

        public UsersController(IUsersRepo users)
        {
            _users = users;
        }
        
        [HttpGet("")]
        public ActionResult<UserListResponse> Search([FromQuery] UserSearchRequest searchRequest)
        {
            var hasAuth = Request.Headers.TryGetValue("Authorization", out var authHeader);
            if(!hasAuth)
            {
                return Unauthorized();
            }
            var authHelper = new AuthHelper(authHeader.ToString(), _users);
            if (!authHelper.IsAuthenticated)
            {
                return Unauthorized();
            }
            var users = _users.Search(searchRequest);
            var userCount = _users.Count(searchRequest);
            return UserListResponse.Create(searchRequest, users, userCount);
        }

        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetById([FromRoute] int id)
        {
            var hasAuth = Request.Headers.TryGetValue("Authorization", out var authHeader);
            if(!hasAuth)
            {
                return Unauthorized();
            }
            var authHelper = new AuthHelper(authHeader.ToString(), _users);
            if (!authHelper.IsAuthenticated)
            {
                return Unauthorized();
            }
            var user = _users.GetById(id);
            return new UserResponse(user);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateUserRequest newUser)
        {
            var hasAuth = Request.Headers.TryGetValue("Authorization", out var authHeader);
            if(!hasAuth)
            {
                return Unauthorized();
            }
            var authHelper = new AuthHelper(authHeader.ToString(), _users);
            if (!authHelper.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = _users.Create(newUser);

            var url = Url.Action("GetById", new { id = user.Id });
            var responseViewModel = new UserResponse(user);
            return Created(url, responseViewModel);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<UserResponse> Update([FromRoute] int id, [FromBody] UpdateUserRequest update)
        {
            var hasAuth = Request.Headers.TryGetValue("Authorization", out var authHeader);
            if(!hasAuth)
            {
                return Unauthorized();
            }
            var authHelper = new AuthHelper(authHeader.ToString(), _users);
            if (!authHelper.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _users.Update(id, update);
            return new UserResponse(user);
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var hasAuth = Request.Headers.TryGetValue("Authorization", out var authHeader);
            if(!hasAuth)
            {
                return Unauthorized();
            }
            var authHelper = new AuthHelper(authHeader.ToString(), _users);
            if (!authHelper.IsAuthenticated)
            {
                return Unauthorized();
            }
            
            _users.Delete(id);
            return Ok();
        }
    }
}