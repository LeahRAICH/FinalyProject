using Tasks.Interfaces;
using Tasks.Models;
using System.Linq;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Tasks.Services
{
    public class TaskService : ITaskService
    {
        List<Task> Tasks { get; }
        private IWebHostEnvironment  webHost;
        private string filePath;

        public TaskService(IWebHostEnvironment webHost){
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }
    
        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Tasks));
        }
        public List<Task> GetAll(long userId)
        {
            Console.WriteLine(userId);

            return Tasks.Where(t => t.AgentId == userId).ToList();
        }
        

     
        public Task Get(long userId, int id) 
        { 
            return Tasks.FirstOrDefault(p => p.Id == id && p.AgentId==userId);
        }

        public void Add(long userId, Task task)
        {
            task.Id = Tasks.Count() + 1;
            task.AgentId = userId;
            Tasks.Add(task);
            saveToFile();
        }

        public void Delete(long userId, int id)
        {
            var task = Get( userId, id);
            if (task is null)
                return;

            Tasks.Remove(task);
            saveToFile();
        }

        public void Update(long userId, Task task)
        {
            var index = Tasks.FindIndex(t => t.AgentId == userId &&  t.Id == task.Id);
            if (index == -1)
                return;
            task.AgentId=userId;
            Tasks[index] = task;
            saveToFile();
        }

        public int Count(long userId) 
        { 
            return GetAll(userId).Count();
        }
    }
}
