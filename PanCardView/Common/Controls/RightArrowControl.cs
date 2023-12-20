using System.ComponentModel;
using static PanCardView.Resources.ResourcesInfo;

namespace PanCardView.Controls
{
    public class RightArrowControl : ArrowControl
    {
        public RightArrowControl()
        {
            HorizontalOptions = LayoutOptions.End;
        }

        protected override ImageSource DefaultImageSource => WhiteRightArrowImageSource;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new static void Preserve()
        {
        }
    }
}
