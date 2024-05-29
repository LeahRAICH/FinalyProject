using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tasks.Models;
using Tasks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using Tasks.services;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace Tasks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class userController : ControllerBase
    {

        private readonly long userId;

        IUserService userService;
        public userController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.userId = long.Parse(httpContextAccessor.HttpContext.User?.FindFirst("userId")?.Value);

        }
        [HttpGet]
        [Authorize(Policy = "Manager")]
        public ActionResult<List<TaskUser>> GetAll() => userService.GetAll();

        [HttpGet("{id}")]
        [Authorize(Policy = "TaskUser")]
        public ActionResult<TaskUser> GetMyUser()
        {
            var user = userService.Get(userId);
            if (user == null)
                return NotFound();
            return user;
        }

       
        [HttpPost]
        [Authorize(Policy = "Manager")]
        public ActionResult Post([FromBody] TaskUser user)
        {
            userService.Post(user);
            return CreatedAtAction(nameof(Post), new { Id = user.UserId }, user);
        }

        [HttpDelete]
        [Authorize(Policy = "Manager")]
        public ActionResult Delete(int id)
        {
            var user = userService.Get(id);
            if (user == null)
                return NotFound();
            userService.Delete(id);
            return NoContent();
        }

    }

}