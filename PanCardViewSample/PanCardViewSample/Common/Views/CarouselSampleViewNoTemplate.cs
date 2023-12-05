using CardCarouselView = PanCardView.CarouselView;
namespace PanCardViewSample.Views
{
    public class CarouselSampleViewNoTemplate : ContentPage
    {
        public CarouselSampleViewNoTemplate()
        {
            Title = "CarouselSampleViewNoTemplate";

            var carousel = new CardCarouselView
            {
                HeightRequest = 200,
                ItemsSource = new[] {
                    new BoxView { Color = Colors.Red },
                    new BoxView { Color = Colors.Blue},
                    new BoxView { Color = Colors.Yellow}
                },
                IsCyclical = false
            };

            var button = new Button { Text = "Select last" };
            button.Clicked += (sender, args) => { carousel.SelectedIndex = 2; };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children = {
                        new StackLayout
                        {
                            Children = {
                                carousel
                            }
                        },
                        button
                    }
                }
            };
        }
    }
}