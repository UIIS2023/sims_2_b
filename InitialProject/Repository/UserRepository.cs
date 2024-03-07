using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository
{
    public class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
          
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public User GetById(int id)
        {
            
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User Update(User user)
        {

            User current = _users.Find(u => u.Id == user.Id);
            int index = _users.IndexOf(current);
            _users.Remove(current);
            _users.Insert(index, user);
            _serializer.ToCSV(FilePath, _users);
            return user;
        }
        
        public void Delete(User user)
        {
             _users = _serializer.FromCSV(FilePath);
             User founded = _users.Find(c => c.Id == user.Id);
            _users.Remove(founded);
             _serializer.ToCSV(FilePath, _users);
            
        }
        public string GetImageUrlByUserId(int id)
		{
            foreach(User user in _users)
			{
                if (user.Id == id)
				{
                    string url = user.ImageUrl;
                    return url;
				}
			}
            return null; 
		}
    }
}
