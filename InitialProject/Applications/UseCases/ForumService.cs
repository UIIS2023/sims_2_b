using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
    public class ForumService
    {
		private readonly IForumRepository forumRepository;
		private readonly LocationService locationService;


		public ForumService()
		{
			forumRepository = Inject.CreateInstance<IForumRepository>();
			locationService=new LocationService();


				}
		

		public Forums Save(Forums f, Comment comment)
		{
			int nextForumId = GetNextForumId();
			f.Id = nextForumId;
			f.Comments.Add(comment);

			return forumRepository.SaveWithComment(f,comment);
		}
	



		private void BindData(List<Forums> forums)
		{
			

			foreach (Forums forum in forums)
			{
				forum.Location = locationService.GetById(forum.LocationId);
			}
		}

		private void BindParticularData(Forums forum)
		{
			
			forum.Location = locationService.GetById(forum.LocationId);
		}

		public List<Forums> GetAll()
		{
			
	
			List<Forums> forums = new List<Forums>();
			forums = forumRepository.GetAll();

			if (forums.Count > 0)
			{
				BindData(forums);
			}

			return forums;
		}
	
		public int GetNextForumId()
		{
			List<Forums> allForums = forumRepository.GetAll();
			if (allForums.Count > 0)
			{
				return allForums.Max(f => f.Id) + 1;
			}
			else
			{
				return 1;
			}
		}



		public void Delete(Forums f)
		{

			forumRepository.Delete(f);
		}



		public Forums Update(Forums f)
		{
			return forumRepository.Update(f);
		}



		public List<Forums> GetByUser(User user)
		{
		  List<Forums> forums = new List<Forums>();
		  forums=forumRepository.GetByUser(user);

			if (forums.Count > 0)
			{
				BindData(forums);
			}

			return forums;
		}

		public Forums GetById(int id)
		{
			Forums forum = forumRepository.GetById(id);
			if(forum != null)
			{
				BindParticularData(forum);
			}

			return forum;
		}

		public List<Forums> GetAvailableForums(User user)
		{
			AccommodationService accommodationService = new AccommodationService();
			List<Location> uniqueLocations = locationService.GetUniqueLocations(accommodationService.GetByUser(user));
			List<Forums> allForums = forumRepository.GetAll();
			BindData(allForums);

			List<Forums> availableForums = new List<Forums>();

			foreach(Forums f in allForums)
			{
				if (uniqueLocations.Contains(f.Location) &&  !f.IsClosed)
				{
					availableForums.Add(f);
				}
			}

			return availableForums;
		}

	}
}
