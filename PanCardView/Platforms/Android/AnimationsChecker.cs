using PanCardView.Utility;
using Android.Provider;

namespace PanCardView.Droid
{
    public class AnimationsChecker : IAnimationsChecker
    {
        public bool AreAnimationsEnabled
        {
            get
            {
                var resolver = Android.App.Application.Context.ContentResolver;
                try
                {
                    var scale = Settings.Global.AnimatorDurationScale;
                    return Settings.Global.GetFloat(resolver, scale, 1) > 0;
                }
                catch
                {
                    try
                    {
#pragma warning disable
                        var scale = Settings.System.AnimatorDurationScale;
#pragma warning restore
                        return Settings.System.GetFloat(resolver, scale, 1) > 0;
                    }
                    catch
                    {
                        return true;
                    }
                }
            }
        }
    }
}