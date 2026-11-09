namespace PanCardViewSample.ViewModels
{
	public sealed class CardSampleItem
	{
		public CardSampleItem(string source, int index, Color color, string title = "")
		{
			Source = ImageSource.FromUri(new Uri(source));
			Ind = index;
			Color = color;
			Title = title;
		}

		public ImageSource Source { get; }

		public int Ind { get; }

		public Color Color { get; }

		public string Title { get; }
	}
}
