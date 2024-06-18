using PanCardViewSample.Views;


namespace PanCardViewSample
{

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new StartPage());
        }
    }

    public class StartPage : ContentPage
    {
        public StartPage()
        {
            Title = "PanCardViewSample";
            BackgroundColor = Colors.White;

            var toCardsBtn = new Button { Text = "CardsView Items", FontSize = 20, TextColor = Colors.Black };
            toCardsBtn.Clicked += (sender, e) =>
            {
                this.Navigation.PushAsync(new CardsSampleView());
            };

            var toCoverFlowBtn = new Button { Text = "CoverFlowView", FontSize = 20, TextColor = Colors.Black };
            toCoverFlowBtn.Clicked += (sender, e) =>
            {
                this.Navigation.PushAsync(new CoverFlowSampleXamlView());
            };

            var toCarouselNestedBtn = new Button { Text = "CarouselView Nested" };
            toCarouselNestedBtn.Clicked += (sender, e) =>
            {
                this.Navigation.PushAsync(new CarouselSampleNestedXamlView());
            };

            var toCarouselScrollBtn = new Button { Text = "CarouselView scroll" };
            toCarouselScrollBtn.Clicked += (sender, e) =>
            {
                this.Navigation.PushAsync(new CarouselSampleSrollView());
            };

            var toCarouselNoTemplateBtn = new Button { Text = "CarouselView No template" };
            toCarouselNoTemplateBtn.Clicked += (sender, e) =>
            {
                this.Navigation.PushAsync(new CarouselSampleViewNoTemplate());
            };

            var toCarouselXamlBtn = new Button { Text = "CarouselView Xaml", FontSize = 20, TextColor = Colors.Black };
            toCarouselXamlBtn.Clicked += (sender, e) =>
            {
                this.Navigation.PushAsync(new CarouselSampleXamlView());
            };

            var toCarouselIndicatorXamlBtn = new Button { Text = "CarouselView Indicator Xaml", FontSize = 20, TextColor = Colors.Black };
            toCarouselIndicatorXamlBtn.Clicked += (sender, e) =>
            {
                this.Navigation.PushAsync(new CarouselSampleIndicatorXamlView());
            };

            var toCarouselListBtn = new Button { Text = "Carousel ListView" };
            toCarouselListBtn.Clicked += (sender, e) =>
            {
                this.Navigation.PushAsync(new CarouselSampleListView());
            };

            var toCubeBtn = new Button { Text = "CubeView Xaml", FontSize = 20, TextColor = Colors.Black };
            toCubeBtn.Clicked += (sender, e) =>
            {
                this.Navigation.PushAsync(new CubeSampleXamlView());
            };

            var toXFIndicatorViewBtn = new Button { Text = "XF IndicatorView" };
            toXFIndicatorViewBtn.Clicked += (sender, e) =>
            {
                this.Navigation.PushAsync(new CarouselSampleXamlViewXFIndicatorView());
            };

            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Margin = 20,
                    Spacing = 10,
                    Children = {
                        toCardsBtn,
                        toCarouselXamlBtn,
                        toCarouselIndicatorXamlBtn,
                        toCoverFlowBtn,
                        toCubeBtn,
                        toCarouselNestedBtn,
                        toCarouselScrollBtn,
                        toCarouselNoTemplateBtn,
                        toCarouselListBtn,
                        toXFIndicatorViewBtn
                    }
                }
            };
        }
    }
}