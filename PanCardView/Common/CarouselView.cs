using PanCardView.Processors;
using System.ComponentModel;

namespace PanCardView
{
    public class CarouselView : CardsView
    {
        public CarouselView() : this(new CarouselProcessor())
        {
        }

        public CarouselView(IProcessor processor) : base(processor)
        {
            // NOTE: For some reason setting this to True
            // Makes the view invisible on Android
            // Probably it's a MAUI bug
            IsClippedToBounds = 
                DeviceInfo.Platform == DevicePlatform.MacCatalyst || 
                DeviceInfo.Platform == DevicePlatform.iOS;
        }

        protected override double DefaultMoveSizePercentage => .3;

        protected override bool DefaultIsCyclical => true;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new static void Preserve()
        {
        }
    }
}
