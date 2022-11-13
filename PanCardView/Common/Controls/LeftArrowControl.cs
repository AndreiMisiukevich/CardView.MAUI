using System.ComponentModel;
using Microsoft.Maui;
using static PanCardView.Resources.ResourcesInfo;

using AbsoluteLayout = Microsoft.Maui.Controls.Compatibility.AbsoluteLayout;

namespace PanCardView.Controls
{
    public class LeftArrowControl : ArrowControl
    {
        public LeftArrowControl()
        {
            IsRight = false;
            AbsoluteLayout.SetLayoutBounds(this, new Rect(0, .5, -1, -1));
        }

        protected override ImageSource DefaultImageSource => WhiteLeftArrowImageSource;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new static void Preserve()
        {
        }
    }
}
