using Microsoft.Maui;
using Microsoft.Maui.Layouts;

namespace PanCardViewSample.CardsFactory
{
	public class DefaultCardItemView : AbsoluteLayout
	{
		public DefaultCardItemView()
		{
			var tapGesture = new TapGestureRecognizer();
			tapGesture.Tapped += (s, e) => Application.Current.MainPage.DisplayAlert("Tap!", null, "Ok");
			GestureRecognizers.Add(tapGesture);

			var frame = new Frame
			{
				Padding = 0,
				HasShadow = false,
				CornerRadius = 10,
				IsClippedToBounds = true
			};
			frame.SetBinding(BackgroundColorProperty, "Color");

            AbsoluteLayout.SetLayoutFlags(frame, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(frame, new Rect(.5, .5, 300, 300));
            Children.Add(frame);

			var image = new Image
			{
				Aspect = Aspect.AspectFill
			};

			image.SetBinding(Image.SourceProperty, "Source");

			frame.Content = image;
		}
	}
}