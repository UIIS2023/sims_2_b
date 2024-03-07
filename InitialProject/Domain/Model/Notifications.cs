using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
	public class Notifications : ISerializable
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Title { get; set; } 
		public string Content { get; set; }
		public NotificationType NotifType { get; set; }
		public bool IsRead { get; set; }
		public DateOnly NotifDate { get; set; }

		public Notifications( int userId,string title, string content,NotificationType notifType, bool isRead, DateOnly notifDate)
		{
			
			UserId = userId;
			Title = title;
			Content = content;
			NotifType = notifType;
			IsRead = isRead;
			NotifDate = notifDate;

		}

		public Notifications()
		{

		}


		public void FromCSV(string[] values)
		{
			Id = int.Parse(values[0]);
			UserId = int.Parse(values[1]);
			Title = values[2];
			Content = values[3];
			NotifType = (NotificationType)Enum.Parse(typeof(NotificationType), values[4]);
			IsRead = bool.Parse(values[5]);
			NotifDate = DateOnly.Parse(values[6]);

		}

		public string[] ToCSV()
		{
			string[] csvValues =
			{
				Id.ToString(),
				UserId.ToString(),
				Title,
				Content,
				NotifType.ToString(),
				IsRead.ToString(),
				NotifDate.ToString()

			};
			return csvValues;
		}

		
	}
}
