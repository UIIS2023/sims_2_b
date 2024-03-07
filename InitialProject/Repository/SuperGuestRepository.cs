using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository
{
    public class SuperGuestRepository: ISuperGuestRepository
    {
        public const string FilePath = "../../../Resources/Data/superguest.csv";

        private readonly Serializer<SuperGuest> _serializer;

        private List<SuperGuest> superGuests;

        public SuperGuestRepository()
        {
            _serializer = new Serializer<SuperGuest>();
            superGuests = _serializer.FromCSV(FilePath);
        }

        public List<SuperGuest> GetByUser(User user)
        {
            return superGuests.FindAll(a => a.GuestId == user.Id);
        }
        public List<SuperGuest> GetAll()
        {
            return superGuests;
        }

        public SuperGuest Save(SuperGuest request)
        {
            request.Id = NextId();
            superGuests = _serializer.FromCSV(FilePath);
            superGuests.Add(request);
            _serializer.ToCSV(FilePath, superGuests);
            return request;
        }

        public int NextId()
        {

            if (superGuests.Count < 1)
            {
                return 1;
            }
            return superGuests.Max(c => c.Id) + 1;
        }


        public void Delete(SuperGuest request)
        {

            SuperGuest founded = superGuests.Find(r => r.Id == request.Id);
            superGuests.Remove(founded);
            _serializer.ToCSV(FilePath, superGuests);
        }

        public SuperGuest Update(SuperGuest request)
        {

            SuperGuest current = superGuests.Find(r => r.Id == request.Id);
            int index = superGuests.IndexOf(current);
            superGuests.Remove(current);
            superGuests.Insert(index, request);
            _serializer.ToCSV(FilePath, superGuests);
            return request;
        }

        public SuperGuest GetById(int id)
        {

            return superGuests.Find(r => r.Id == id);
        }

    }
}
