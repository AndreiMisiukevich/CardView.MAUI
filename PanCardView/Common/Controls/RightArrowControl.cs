using Microsoft.Maui;
using System.ComponentModel;
using static PanCardView.Resources.ResourcesInfo;

namespace PanCardView.Controls
{
    public class RightArrowControl : ArrowControl
    {
        public RightArrowControl()
        {
            AbsoluteLayout.SetLayoutBounds(this, new Rect(1, .5, -1, -1));
        }

        protected override ImageSource DefaultImageSource => WhiteRightArrowImageSource;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new static void Preserve()
        {
        }
    }
}
