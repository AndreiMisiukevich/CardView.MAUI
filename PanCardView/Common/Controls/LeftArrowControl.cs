using System.ComponentModel;
using static PanCardView.Resources.ResourcesInfo;

namespace PanCardView.Controls
{
    public class LeftArrowControl : ArrowControl
    {
        public LeftArrowControl()
        {
            IsRight = false;
            HorizontalOptions = LayoutOptions.Start;
        }

        protected override ImageSource DefaultImageSource => WhiteLeftArrowImageSource;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new static void Preserve()
        {
        }
    }
}
