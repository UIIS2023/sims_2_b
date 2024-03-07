using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InitialProject.Repository
{

    internal class ImageRepository : IImageRepository
	{
        private const string FilePath = "../../../Resources/Data/images.csv";

        private readonly Serializer<Image> _serializer;

        private List<Image> _images;

        public ImageRepository()
        {
            _serializer = new Serializer<Image>();
            _images = _serializer.FromCSV(FilePath);
        }

        public List<Image> GetAll()
        {
            return _images;
        }

        public Image Save(Image image)
        {
            image.Id = NextId();
            _images = _serializer.FromCSV(FilePath);
            _images.Add(image);
            _serializer.ToCSV(FilePath, _images);
            return image;
        }

        public int NextId()
        {
           
            if (_images.Count < 1)
            {
                return 1;
            }
            return _images.Max(c => c.Id) + 1;
        }

        public void Delete(Image image)
        {
            

            Image founded = _images.Find(a => a.Id == image.Id);

            _images.Remove(founded);
            _serializer.ToCSV(FilePath, _images);
        }

        public Image Update(Image image)
        {
            

            Image current = _images.Find(a => a.Id == image.Id);

            int index = _images.IndexOf(current);
            _images.Remove(current);
            _images.Insert(index, image);       
            _serializer.ToCSV(FilePath, _images);
            return image;
        }

        public List<String> GetUrlByTourId(int id)
        {
            List<String> url = new List<String>();

            foreach(Image image in _images)
            {
                if (image.IdTour == id)
                {
                    url.Add(image.Url);
                }
            }
            return url;
        }


        public string GetFirstUrlByTourId(int id)
        {
            foreach (Image image in _images)
            {
                if (image.IdTour == id)
                {
                    return image.Url;
                }
            }
            return null;
        }

        public List<String> GetUrlByAccommodationId(int id)
        {
            List<String> urlList = new List<String>();
            
            foreach(Image image in _images)
			{
				if (image.IdAccommodation == id)
				{
                    urlList.Add(image.Url);
				}
			}
            return urlList;
		}

        public List<String> GetUrlRateId(int id)
        {
            List<String> urlList = new List<String>();

            foreach (Image image in _images)
            {
                if (image.IdOwner == id)
                {
                    urlList.Add(image.Url);
                }
            }
            return urlList;
        }

        public Image GetById(int id)
		{
            
            return _images.Find(i => i.Id == id);
        }
        
        public void StoreImage(Accommodation savedAccommodation, string ImageUrl)
        {
            foreach (string urls in ImageUrl.Split(','))
            {
                Image image1 = new Image(urls, savedAccommodation.Id, 0,0);
                image1.Id = NextId();
                _images = _serializer.FromCSV(FilePath);
                _images.Add(image1);
                _serializer.ToCSV(FilePath, _images);
            }
        }

        public void StoreImageTourGuideReview(TourGuideReview savedTourGuideReview, string ImageUrl)
        {
            foreach (string urls in ImageUrl.Split(','))
            {
                Image image1 = new Image(urls, 0, savedTourGuideReview.IdTour,0);
                image1.Id = NextId();
                _images = _serializer.FromCSV(FilePath);
                _images.Add(image1);
                _serializer.ToCSV(FilePath, _images);
            }
        }

        public void StoreImageOwnerReview(OwnerReview ownerReview, string ImageUrl)
        {
            foreach (string urls in ImageUrl.Split(','))
            {
                Image image1 = new Image(urls, 0,0, ownerReview.Id);
                image1.Id = NextId();
                _images = _serializer.FromCSV(FilePath);
                _images.Add(image1);
                _serializer.ToCSV(FilePath, _images);
            }
        }
        
    }
}
