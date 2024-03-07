using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IForumRepository:IRepository<Forums>
    {
        public List<Forums> GetByUser(User user);
        public Forums SaveWithComment(Forums forum, Comment comment);

    }
}
