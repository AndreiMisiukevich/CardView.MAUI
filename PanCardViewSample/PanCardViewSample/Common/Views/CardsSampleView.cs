using PanCardView;
using PanCardViewSample.CardsFactory;
using PanCardViewSample.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Layouts;

namespace PanCardViewSample.Views
{
	public class CardsSampleView : ContentPage
	{
		public CardsSampleView()
		{
			var cardsView = new CardsView
			{
				ItemTemplate = new DataTemplate(() => new DefaultCardItemView()),
				BackgroundColor = Colors.Black.MultiplyAlpha(.9f)
			};
			AbsoluteLayout.SetLayoutFlags(cardsView, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(cardsView, new Rect(0, 0, 1, 1));

			var prevItem = new ToolbarItem
			{
				Text = "**Prev**",
				CommandParameter = false
			};
			prevItem.SetBinding(MenuItem.CommandProperty, nameof(CardsSampleViewModel.PanPositionChangedCommand));

			var nextItem = new ToolbarItem
			{
				Text = "**Next**",
				CommandParameter = true
			};
			nextItem.SetBinding(MenuItem.CommandProperty, nameof(CardsSampleViewModel.PanPositionChangedCommand));

			ToolbarItems.Add(prevItem);
			ToolbarItems.Add(nextItem);

			cardsView.SetBinding(CardsView.ItemsSourceProperty, nameof(CardsSampleViewModel.Items));
			cardsView.SetBinding(CardsView.SelectedIndexProperty, nameof(CardsSampleViewModel.CurrentIndex));

			Title = "Cards View";


			var removeButton = new Button
			{
				Text = "Remove current",
				FontAttributes = FontAttributes.Bold,
				TextColor = Colors.Black,
				BackgroundColor = Colors.Yellow.MultiplyAlpha(.7f),
				Margin = new Thickness(0, 0, 0, 10)
			};

			removeButton.SetBinding(Button.CommandProperty, nameof(CardsSampleViewModel.RemoveCurrentItemCommand));

			AbsoluteLayout.SetLayoutFlags(removeButton, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds(removeButton, new Rect(.5, 1, 150, 40));



			Content = new AbsoluteLayout()
			{
				Children =
				{
					cardsView,
					removeButton
				}
			};

			BindingContext = new CardsSampleViewModel();
		}
	}
}
