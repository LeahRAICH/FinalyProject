using Tasks.Models;
using System.Collections.Generic;

namespace Tasks.Interfaces{

    public interface IUserService{
    
        List<TaskUser> GetAll();
        TaskUser Get(long userId);
        void Post(TaskUser u);
        void Delete(int id);
    }

}