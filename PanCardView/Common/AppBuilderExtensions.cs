using Microsoft.Maui.Controls.Compatibility.Hosting;
using PanCardView;
using PanCardView.Controls;
using PanCardView.Utility;
using CarouselView = PanCardView.CarouselView;


namespace PanCardView
{
    public static class AppBuilderExtensions
    {
        public static MauiAppBuilder UseCardsView(this MauiAppBuilder builder)
        {
            CardsView.Preserve();
            CarouselView.Preserve();
            CubeView.Preserve();
            CoverFlowView.Preserve();
            ArrowControl.Preserve();
            LeftArrowControl.Preserve();
            RightArrowControl.Preserve();
            CircleFrame.Preserve();
            IndicatorItemView.Preserve();
            IndicatorsControl.Preserve();
            TabsControl.Preserve();

#if ANDROID
            DependencyService.RegisterSingleton<IAnimationsChecker>(new PanCardView.Droid.AnimationsChecker());
#endif

            return builder.UseMauiCompatibility().ConfigureMauiHandlers((handlers) => {
#if ANDROID
                handlers.AddCompatibilityRenderer(typeof(CardsView), typeof(PanCardView.Droid.CardsViewRenderer));
#endif
#if IOS
                handlers.AddCompatibilityRenderer(typeof(CardsView), typeof(PanCardView.iOS.CardsViewRenderer));
#endif
            });
        }
    }
}