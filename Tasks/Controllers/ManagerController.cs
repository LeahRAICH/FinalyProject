using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tasks.Models;
using Tasks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using Tasks.Services;
using Tasks.services;
namespace Tasks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagerController: ControllerBase
    {
        IUserService userService;
        
        private List<TaskUser> users;
        public ManagerController(IUserService userService)
        {
            this.userService = userService;
            this.users=this.userService.GetAll();
        }
        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] TaskUser User)
        {
            var dt = DateTime.Now;

            var user = users.FirstOrDefault(u =>
                u.Username == User.Username 
                && u.Password == User.Password
            );        

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("UserType", user.Manager ? "Manager" : "TaskUser"),
                new Claim("userId", user.UserId.ToString()),

            };
            if(user.Manager)
                claims.Add(new Claim("UserType","TaskUser"));

            var token = TaskTokenService.GetToken(claims);

            return new OkObjectResult(TaskTokenService.WriteToken(token));
        }
    }
}