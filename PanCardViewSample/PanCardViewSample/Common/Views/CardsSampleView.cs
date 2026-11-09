using PanCardView;
using PanCardViewSample.CardsFactory;
using PanCardViewSample.ViewModels;
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
			prevItem.SetBinding(MenuItem.CommandProperty, static (CardsSampleViewModel viewModel) => viewModel.PanPositionChangedCommand, BindingMode.OneWay);

			var nextItem = new ToolbarItem
			{
				Text = "**Next**",
				CommandParameter = true
			};
			nextItem.SetBinding(MenuItem.CommandProperty, static (CardsSampleViewModel viewModel) => viewModel.PanPositionChangedCommand, BindingMode.OneWay);

			ToolbarItems.Add(prevItem);
			ToolbarItems.Add(nextItem);

			cardsView.SetBinding(CardsView.ItemsSourceProperty, static (CardsSampleViewModel viewModel) => viewModel.Items, BindingMode.OneWay);
			cardsView.SetBinding(CardsView.SelectedIndexProperty, static (CardsSampleViewModel viewModel) => viewModel.CurrentIndex, BindingMode.TwoWay);

			Title = "Cards View";


			var removeButton = new Button
			{
				Text = "Remove current",
				FontAttributes = FontAttributes.Bold,
				TextColor = Colors.Black,
				BackgroundColor = Colors.Yellow.MultiplyAlpha(.7f),
				Margin = new Thickness(0, 0, 0, 10)
			};

			removeButton.SetBinding(Button.CommandProperty, static (CardsSampleViewModel viewModel) => viewModel.RemoveCurrentItemCommand, BindingMode.OneWay);

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
