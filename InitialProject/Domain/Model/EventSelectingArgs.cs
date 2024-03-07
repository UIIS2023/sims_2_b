using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class EventSelectingArgs : EventArgs
    {
        public Tour SelectedTour { get; set; }
        public EventSelectingArgs(Tour tour)
        {
            SelectedTour = tour;
        }
    }
}
