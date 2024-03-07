using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class ForumRepository:IForumRepository
    {
        private const string FilePath = "../../../Resources/Data/forums.csv";

        private readonly Serializer<Forums> _serializer;

        private List<Forums> _forums;

        public ForumRepository()
        {
            _serializer = new Serializer<Forums>();
            _forums = _serializer.FromCSV(FilePath);
        }
       
        public List<Forums> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Forums Save(Forums forum)
        {

            forum.Id = NextId();
            _forums = _serializer.FromCSV(FilePath);
            _forums.Add(forum);
            _serializer.ToCSV(FilePath, _forums);
            return forum;
        }


        public Forums SaveWithComment(Forums forum, Comment comment)
        {
            int nextForumId = NextId();
            forum.Id = nextForumId;

            // Add the comment to the forum
            forum.Comments.Add(comment);

            // Save the forum
            _forums.Add(forum);
            _serializer.ToCSV(FilePath, _forums);

            return forum;
        }

        public int NextId()
        {

            if (_forums.Count < 1)
            {
                return 1;
            }
            return _forums.Max(f => f.Id) + 1;
        }

        public List<Forums> GetByUser(User user)
        {

            return _forums.FindAll(f => f.IdUser == user.Id);

        }

        public void Delete(Forums forum)
        {
            Forums founded = _forums.Find(c => c.Id == forum.Id);
            _forums.Remove(founded);
            _serializer.ToCSV(FilePath, _forums);
        }

        public Forums Update(Forums forum)
        {
            
            Forums current = _forums.Find(c => c.Id == forum.Id);
            int index = _forums.IndexOf(current);
            _forums.Remove(current);
            _forums.Insert(index, forum);       
            _serializer.ToCSV(FilePath, _forums);
            return forum;
        }

        public Forums GetById(int id)
        {
            return _forums.Find(g => g.Id == id);
        }
       

    }
}
