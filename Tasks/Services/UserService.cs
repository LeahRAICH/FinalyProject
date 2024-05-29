using Tasks.Models;
using Tasks.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
namespace Tasks.services
{

    public class userService : IUserService
    {

        List<TaskUser> users { get; }

        private IWebHostEnvironment webHost;
        private string filePath;

        public userService(IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor)
        { 
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "data", "users.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                users = JsonSerializer.Deserialize<List<TaskUser>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(users));
        }

        public List<TaskUser> GetAll() => users;

        public TaskUser Get(long userId)=>users?.FirstOrDefault(u=>u.UserId==userId);

        public void Post(TaskUser u)
        {
            u.UserId = users[users.Count() - 1].UserId + 1;
            u.Manager = false;
            users.Add(u);
            saveToFile();
        }

        public void Delete(int id){
            var user=Get(id);
            if(user is null)
                return;
            users.Remove(user);
            saveToFile();
        }
    }
}