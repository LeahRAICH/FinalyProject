using System.Collections.Generic;
using Tasks.Models;

namespace Tasks.Interfaces
{
    public interface ITaskService
    {
        List<Task> GetAll(long userId);
        Task Get(long userId,int id);
        void Add(long userId,Task Task);
        void Delete(long userId,int id);
        void Update(long userId,Task Task);
        int Count (long userId);
    }
}