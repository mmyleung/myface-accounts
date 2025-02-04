﻿using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using MyFace.Helpers;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/posts")]
    public class PostsController : ControllerBase
    {    
        private readonly IPostsRepo _posts;
        private readonly  IUsersRepo _usersRepo;

        public PostsController(IPostsRepo posts, IUsersRepo usersRepo)
        {
            _posts = posts;
            _usersRepo = usersRepo;
        }
        
        [HttpGet("")]
        public ActionResult<PostListResponse> Search([FromQuery] PostSearchRequest searchRequest)
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
            var posts = _posts.Search(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return PostListResponse.Create(searchRequest, posts, postCount);
        }

        [HttpGet("{id}")]
        public ActionResult<PostResponse> GetById([FromRoute] int id)
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
            var post = _posts.GetById(id);
            return new PostResponse(post);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreatePostRequest newPost)
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
            
            var userId = _usersRepo.GetByUsername(newPost.Username).Id;
            var post = _posts.Create(newPost, userId);

            var url = Url.Action("GetById", new { id = post.Id });
            var postResponse = new PostResponse(post);
            return Created(url, postResponse);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<PostResponse> Update([FromRoute] int id, [FromBody] UpdatePostRequest update)
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

            var post = _posts.Update(id, update);
            return new PostResponse(post);
        }

        [HttpDelete("{id}/delete")]
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
            if (!authHelper.IsAdmin)
            {
                return Unauthorized();
            }
            _posts.Delete(id);
            return Ok();
        }
    }
}