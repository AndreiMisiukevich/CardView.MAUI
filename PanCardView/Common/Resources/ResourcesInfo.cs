using Microsoft.Maui;
using System.Reflection;

namespace PanCardView.Resources
{
    public static class ResourcesInfo
    {
        private const string ResourcesPath = "PanCardView.Resources";
        private const string WhiteRightArrowResourceName = "right_arrow.png";
        private const string WhiteLeftArrowResourceName = "left_arrow.png";
        private const string BlackRightArrowResourceName = "right_arrow_black.png";
        private const string BlackLeftArrowResourceName = "left_arrow_black.png";

        private static Assembly ResourceAssembly => typeof(ResourcesInfo).GetTypeInfo().Assembly;

        public static ImageSource WhiteRightArrowImageSource => FromResource(WhiteRightArrowResourceName);

        public static ImageSource WhiteLeftArrowImageSource => FromResource(WhiteLeftArrowResourceName);

        public static ImageSource BlackRightArrowImageSource => FromResource(BlackRightArrowResourceName);

        public static ImageSource BlackLeftArrowImageSource => FromResource(BlackLeftArrowResourceName);

        private static ImageSource FromResource(string resourceName)
            => resourceName;
    }
}
