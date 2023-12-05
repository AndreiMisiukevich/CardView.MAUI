using Microsoft.Maui.Handlers;

namespace PanCardView;

public partial class CardsViewHandler : LayoutHandler
{
    public static IPropertyMapper<CardsView, LayoutHandler> PropertyMapper = new PropertyMapper<CardsView, LayoutHandler>(Mapper)
    {
#if IOS
        [nameof(CardsView.IsVerticalSwipeEnabled)] = MapIsVerticalSwipeEnabled,
        [nameof(CardsView.IsUserInteractionEnabled)] = MapIsUserInteractionEnabled
#endif
    };

    public CardsViewHandler() : base(PropertyMapper)
    {
    }
}