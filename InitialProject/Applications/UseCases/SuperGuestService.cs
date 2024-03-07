using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
    public class SuperGuestService
    {
		private readonly ISuperGuestRepository superGuestRepository;
        private readonly UserService userService;
		


		public SuperGuestService()
		{
			superGuestRepository = Inject.CreateInstance<ISuperGuestRepository>();
            userService = new UserService();
			

		}

		public SuperGuest Save(SuperGuest superGuest)
		{
			return superGuestRepository.Save(superGuest);
		}

		

		public List<SuperGuest> GetAll()
		{
			return superGuestRepository.GetAll();
		}


		public void Delete(SuperGuest superGuest)
			{

				superGuestRepository.Delete(superGuest);
			}

	

		public SuperGuest Update(SuperGuest superGuest)
		{
			return superGuestRepository.Update(superGuest);
		}

		

		public List<SuperGuest> GetByUser(User user)

		{
			return superGuestRepository.GetByUser(user);
		}

		public SuperGuest GetById(int id)
		{

			return superGuestRepository.GetById(id);
		}

		public SuperGuest GetSuperGuest(int id)
        {
			foreach(SuperGuest superGuest in superGuestRepository.GetAll())
            {
				
					if (superGuest.GuestId == id)
						return superGuest;
				
            }

			return null;
        }

		public int LastYearReservations(ObservableCollection<AccommodationReservation> AccommodationsReservationList)
        {
            var today = DateTime.Today;
            var oneYearAgo = today.AddYears(-1);

            DateOnly startDate1 = DateOnly.FromDateTime(today);
            DateOnly endDate1 = DateOnly.FromDateTime(oneYearAgo);

            int reservationsLastYear = 0;
			foreach (AccommodationReservation a in AccommodationsReservationList)
			{
				if (a.StartDate <= startDate1 && a.EndDate >= endDate1 && a.EndDate <= startDate1)
				{
					reservationsLastYear++;
				}
			}
			return reservationsLastYear;
		}


        public (bool IsSuperGuest, bool IsEnabled, int bonusPoints) CheckSuperGuest(User loggedInUser, SuperGuest superGuest, ObservableCollection<AccommodationReservation> reservations)
        {
            var today = DateTime.Today;
            var oneYearAgo = today.AddYears(-1);
            bool IsSuper;
            int bonus;
            DateOnly startDate1 = DateOnly.FromDateTime(today);
            DateOnly endDate1 = DateOnly.FromDateTime(oneYearAgo);
            int reservationsLastYear = LastYearReservations(reservations);
            DateTime SuperGuestExpirationDate;

            if (superGuest != null)
            {
                SuperGuestExpirationDate = superGuest.SuperGuestExpirationDate;
            }
            else
            {
                SuperGuestExpirationDate= DateTime.MinValue;
            }

            bool isEnabled;

            if (reservationsLastYear < 10 && superGuest==null)
            {
                IsSuper = false;
                isEnabled = false;
                bonus = 0;
               
            }

            if (reservationsLastYear >= 10 && SuperGuestExpirationDate < oneYearAgo)
            {
                UpdateSuperGuest(loggedInUser, superGuest, today, out isEnabled);
                IsSuper = true;
                isEnabled = true;
                bonus = 5;
            }
            else if (reservationsLastYear >= 10)
            {
                loggedInUser.IsSuper = true;
                IsSuper = true;
                isEnabled = true;
                bonus = int.Parse(superGuest.Bonus);

            }
            else if (superGuest == null)
            {
                IsSuper = false;
                isEnabled = false;
                bonus = 0;
            }
            else
            {
                ResetSuperGuest(loggedInUser, superGuest, out isEnabled);
                IsSuper = false;
                isEnabled = false;
                bonus = 0;
            }
            return (true, isEnabled, bonus);

        }

        private void ResetSuperGuest(User loggedInUser, SuperGuest superGuest, out bool isEnabled)
        {
            superGuest.SuperGuestExpirationDate = DateTime.MinValue;
            superGuest.Bonus = "0";
            superGuestRepository.Update(superGuest);
            isEnabled = false;
            loggedInUser.IsSuper = false;
        }

        private void UpdateSuperGuest(User loggedInUser, SuperGuest superGuest, DateTime today, out bool isEnabled)
        {
            superGuest.SuperGuestExpirationDate = today.AddYears(1);
            superGuest.Bonus = "5";
            loggedInUser.IsSuper = true;
            isEnabled = true;
            userService.Update(loggedInUser);
            superGuest = new SuperGuest(loggedInUser.Id, superGuest.Bonus, superGuest.SuperGuestExpirationDate);
            superGuestRepository.Save(superGuest);
        }
    }
}
