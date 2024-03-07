using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
	public interface IUserRepository
	{
		User GetByUsername(string username);

		User GetById(int id);

		User Update(User user);

		void Delete(User user);

		string GetImageUrlByUserId(int id);

		List<User> GetAll();
	}
}
