using System.Collections.ObjectModel;

namespace PanCardViewSample.ViewModels
{
	public class CarouselSampleScrollViewModel
	{
		public CarouselSampleScrollViewModel()
		{
			Items = new ObservableCollection<ScrollSampleItem>(Enumerable.Range(1, 3).Select(i => new ScrollSampleItem(i)));
		}

		public ObservableCollection<ScrollSampleItem> Items { get; }
	}

	public sealed class ScrollSampleItem
	{
		public ScrollSampleItem(int number) => Number = number;

		public int Number { get; }
	}
}
