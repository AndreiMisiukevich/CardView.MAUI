using System.Collections.ObjectModel;

namespace PanCardViewSample.ViewModels
{
	public sealed class CarouselSampleNestedViewModel
	{
		public ObservableCollection<CarouselNestedItem> Items { get; } = new ObservableCollection<CarouselNestedItem>
		{
			new CarouselNestedItem
            {
				Text = "FIRST",
				Color = Colors.LightBlue,
				Items = new ObservableCollection<CarouselNestedImageItem>
                {
					new CarouselNestedImageItem { Text = "1/2 NESTED ITEM", Source = ImageSource.FromUri(new Uri("https://picsum.photos/500/500?image=250")) },
					new CarouselNestedImageItem { Text = "2/2 NESTED ITEM", Source = ImageSource.FromUri(new Uri("https://picsum.photos/500/500?image=251")) },
				}
            },
			new CarouselNestedItem
			{
				Text = "SECOND",
				Color = Colors.LightPink,
				Items = new ObservableCollection<CarouselNestedImageItem>
				{
					new CarouselNestedImageItem { Text = "1/2 NESTED ITEM", Source = ImageSource.FromUri(new Uri("https://picsum.photos/500/500?image=252")) },
					new CarouselNestedImageItem { Text = "2/2 NESTED ITEM", Source = ImageSource.FromUri(new Uri("https://picsum.photos/500/500?image=253")) },
				}
			},
			new CarouselNestedItem
			{
				Text = "THIRD",
				Color = Colors.LightSkyBlue,
				Items = new ObservableCollection<CarouselNestedImageItem>
				{
					new CarouselNestedImageItem { Text = "1/2 NESTED ITEM", Source = ImageSource.FromUri(new Uri("https://picsum.photos/500/500?image=254")) },
					new CarouselNestedImageItem { Text = "2/2 NESTED ITEM", Source = ImageSource.FromUri(new Uri("https://picsum.photos/500/500?image=255")) },
				}
			},
			new CarouselNestedItem
			{
				Text = "FOURTH",
				Color = Colors.LightGray,
				Items = new ObservableCollection<CarouselNestedImageItem>
				{
					new CarouselNestedImageItem { Text = "1/2 NESTED ITEM", Source = ImageSource.FromUri(new Uri("https://picsum.photos/500/500?image=256")) },
					new CarouselNestedImageItem { Text = "2/2 NESTED ITEM", Source = ImageSource.FromUri(new Uri("https://picsum.photos/500/500?image=257")) },
				}
			},
			new CarouselNestedItem
			{
				Text = "FIFTH",
				Color = Colors.LightGoldenrodYellow,
				Items = new ObservableCollection<CarouselNestedImageItem>
				{
					new CarouselNestedImageItem { Text = "1/2 NESTED ITEM", Source = ImageSource.FromUri(new Uri("https://picsum.photos/500/500?image=257")) },
					new CarouselNestedImageItem { Text = "2/2 NESTED ITEM", Source = ImageSource.FromUri(new Uri("https://picsum.photos/500/500?image=258")) },
				}
			},
		};

	}

	public sealed class CarouselNestedItem
	{
		public string Text { get; set; }
		public Color Color { get; set; }
		public ObservableCollection<CarouselNestedImageItem> Items { get; set; }
	}

	public sealed class CarouselNestedImageItem
	{
		public string Text { get; set; }
		public Color Color { get; set; }
		public ImageSource Source { get; set; }
	}

	public class ColorItem
    {
		public Color Color { get; set; }
    }
}
