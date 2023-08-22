using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Helpers;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("feed")]
    public class FeedController : ControllerBase
    {
        private readonly IPostsRepo _posts;
        private readonly IUsersRepo _usersRepo;

        public FeedController(IPostsRepo posts)
        {
            _posts = posts;
        }

        [HttpGet("")]
        public ActionResult<FeedModel> GetFeed([FromQuery] FeedSearchRequest searchRequest)
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
            var posts = _posts.SearchFeed(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return FeedModel.Create(searchRequest, posts, postCount);
        }
    }
}
