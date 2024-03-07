using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Converters
{
    public class SortedObservableCollection<T> : ObservableCollection<T>
    {
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            if (index > 0)
            {
                MoveItem(index, 0); // Move the last added item to the first position
            }
        }
    }
}
