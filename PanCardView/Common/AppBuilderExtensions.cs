using PanCardView.Controls;
#if ANDROID
using PanCardView.Utility;
#endif

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
            CircleBorder.Preserve();
            IndicatorItemView.Preserve();
            IndicatorsControl.Preserve();
            TabsControl.Preserve();

#if ANDROID
            DependencyService.RegisterSingleton<IAnimationsChecker>(new Droid.AnimationsChecker());
#endif
            return builder.ConfigureMauiHandlers(handlers =>
            {
#if ANDROID || IOS         
                handlers.AddHandler(typeof(CardsView), typeof(CardsViewHandler));
#endif
            });
        }
    }
}