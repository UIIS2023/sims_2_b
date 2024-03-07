using InitialProject.Applications.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModel
{
    public class RateOwnerViewModel:BindableBase
    {   public Action CloseAction{ get; set; }

		private readonly OwnerReviewService ownerReviewService;
		private readonly ImageRepository _imageRepository;
		public User LogedUser;

		public OwnerReview ownerReview=new OwnerReview();
		public static AccommodationReservation SelectedReservation { get; set; }

		public RateOwnerViewModel(User user,AccommodationReservation reservation)
		{
			
			InitializeCommands();
			SelectedReservation= reservation;
			ownerReviewService = new OwnerReviewService();
			LogedUser= user;
			_imageRepository=new ImageRepository();

		}
		public OwnerReview OwnerReviews
		{
			get { return ownerReview; }
			set
			{
				ownerReview = value;
				OnPropertyChanged("OwnerReviews");
			}
		}

		private bool CanExecute_Command(object parameter)
		{
			return true;
		}

		private void Execute_CancelCommand(object sender)
		{
			CloseAction();
		}

		private void InitializeCommands()
        {
			RateOwner = new RelayCommand(Execute_RateOwner, CanExecute_Command);
			CancelRate = new RelayCommand(Execute_CancelCommand, CanExecute_Command);
			Instruction= new RelayCommand(Execute_Instruction, CanExecute_Command);
		}

        private void Execute_Instruction(object obj)
        {
            throw new NotImplementedException();
        }
		

		

		private void Execute_RateOwner(object obj)
		{
			
			OwnerReviews.Validate();

			if (OwnerReviews.IsValid)
			{
				// Create a new OwnerReview object with the validated values and save it
				
				OwnerReview newReview = new OwnerReview(OwnerReviews.OwnerCorrectness, OwnerReviews.CleanlinessGrade, OwnerReviews.Comment, SelectedReservation.Id, SelectedReservation, LogedUser.Id);
                
				OwnerReview savedReview = ownerReviewService.Save(newReview);

				// Add the saved review to the RateOwnerList collection and store the image
				Guest1MainWindowViewModel.RateOwnerList.Add(savedReview);
				_imageRepository.StoreImageOwnerReview(savedReview, OwnerReviews.ImageUrl);

				CloseAction();
			}
			else
			{
				// Update the view with the validation errors
				OnPropertyChanged(nameof(OwnerReviews));
			}
		}



		private RelayCommand rateOwner;
		public RelayCommand RateOwner
		{
			get { return rateOwner; }
			set
			{
				rateOwner = value;
			}
		}

		private RelayCommand instruction;
		public RelayCommand Instruction
		{
			get { return instruction; }
			set
			{
				instruction = value;
			}
		}

		private RelayCommand cancelCommand;
		public RelayCommand CancelRate
		{
			get { return cancelCommand; }
			set
			{
				cancelCommand = value;
			}
		}

	

		

		
		
		
	}
}
