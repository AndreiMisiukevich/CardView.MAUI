using System.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using PanCardView.Extensions;

namespace PanCardView.Controls
{
    public class CircleBorder : Border
    {
        public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(double), typeof(CircleBorder), 25.0, propertyChanged: (bindable, oldValue, newValue) =>
        {
            bindable.AsCircleBorder().OnSizeUpdated();
        });

        public CircleBorder()
        {
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            StrokeShape = new Ellipse();

            // NOTE: Default Size was set either by bindable property default or
            // applied style which doesn't call property changed. Need to manually update.
            OnSizeUpdated();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void Preserve()
        {
        }

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        protected void OnSizeUpdated()
        {
            var size = Size;
            if(size < 0)
            {
                return;
            }

            try
            {
                BatchBegin();
                HeightRequest = size;
                WidthRequest = size;
            }
            finally
            {
                BatchCommit();
            }
        }
    }
}
