using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Helpers;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/interactions")]
    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionsRepo _interactions;
        private readonly IUsersRepo _usersRepo;

        public InteractionsController(IInteractionsRepo interactions)
        {
            _interactions = interactions;
        }
    
        [HttpGet("")]
        public ActionResult<ListResponse<InteractionResponse>> Search([FromQuery] SearchRequest search)
        {
            var hasAuth = Request.Headers.TryGetValue("Authorization", out var authHeader);
            if(!hasAuth)
            {
                return Unauthorized();
            }
            var authHelper = new AuthHelper(authHeader.ToString(), _usersRepo);
            if (!authHelper.IsAuthenticated)
            {
                return Unauthorized();
            }
            var interactions = _interactions.Search(search);
            var interactionCount = _interactions.Count(search);
            return InteractionListResponse.Create(search, interactions, interactionCount);
        }

        [HttpGet("{id}")]
        public ActionResult<InteractionResponse> GetById([FromRoute] int id)
        {
            var hasAuth = Request.Headers.TryGetValue("Authorization", out var authHeader);
            if(!hasAuth)
            {
                return Unauthorized();
            }
            var authHelper = new AuthHelper(authHeader.ToString(), _usersRepo);
            if (!authHelper.IsAuthenticated)
            {
                return Unauthorized();
            }
            var interaction = _interactions.GetById(id);
            return new InteractionResponse(interaction);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateInteractionRequest newUser)
        {
            var hasAuth = Request.Headers.TryGetValue("Authorization", out var authHeader);
            if(!hasAuth)
            {
                return Unauthorized();
            }
            var authHelper = new AuthHelper(authHeader.ToString(), _usersRepo);
            if (!authHelper.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        
            var interaction = _interactions.Create(newUser);

            var url = Url.Action("GetById", new { id = interaction.Id });
            var responseViewModel = new InteractionResponse(interaction);
            return Created(url, responseViewModel);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var hasAuth = Request.Headers.TryGetValue("Authorization", out var authHeader);
            if(!hasAuth)
            {
                return Unauthorized();
            }
            var authHelper = new AuthHelper(authHeader.ToString(), _usersRepo);
            if (!authHelper.IsAuthenticated)
            {
                return Unauthorized();
            }
            _interactions.Delete(id);
            return Ok();
        }
    }
}
