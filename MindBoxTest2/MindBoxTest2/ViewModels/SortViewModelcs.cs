using MindBoxTest2.Models;

namespace MindBoxTest2.ViewModels
{
    public class SortViewModel
    {
        public SortState ProductSort { get; private set; }
        public SortState Current { get; private set; }

        public SortViewModel(SortState sortOrder)
        {
            ProductSort = sortOrder == SortState.ProductAsc ? SortState.ProductDesc : SortState.ProductAsc;
            Current = sortOrder;
        }
    }
}
