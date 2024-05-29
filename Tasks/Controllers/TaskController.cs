using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tasks.Interfaces;
using Tasks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;

namespace Tasks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "TaskUser")]
    public class TaskController : ControllerBase
    {
        private ITaskService TaskService;
                private readonly long  userId;

        public TaskController(ITaskService taskService,IHttpContextAccessor httpContextAccessor)
        {
            this.TaskService=taskService;
            this.userId=(long.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value));
        }

        
       [HttpGet]
        public  ActionResult<List<Task>> GetAll() =>
        TaskService.GetAll(userId);


        [HttpGet ("{id}")]
        public ActionResult<Task> Get(int id) 
        {   
           var Task = TaskService.Get(userId,id);

            if (Task == null)
                return NotFound();

            return Task;
        }  
        
        [HttpPost] 
        public IActionResult Create(Task Task)
        {
            TaskService.Add(userId,Task);
            return CreatedAtAction(nameof(Create), new {id=Task.Id}, Task);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Task Task)
        {
            if (id != Task.Id)
                return BadRequest();

            var existingTask = TaskService.Get(userId,id);
            if (existingTask is null)
                return  NotFound();

            TaskService.Update(userId,Task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var Task = TaskService.Get(userId,id);
            if (Task is null)
                return  NotFound();

            TaskService.Delete(userId,id);

            return Content(TaskService.Count( userId).ToString());
        }      
    }
}
