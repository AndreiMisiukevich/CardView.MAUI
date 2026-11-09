namespace PanCardView.Extensions;

internal enum CardsViewBindingContextKind
{
    CardsView,
    TypedCustomContext,
    LegacyCustomContext
}

internal static class CardsViewBindingExtensions
{
    internal static CardsViewBindingContextKind GetCardsViewBindingContextKind(this BindableObject bindable)
        => bindable.BindingContext switch
        {
            CardsView => CardsViewBindingContextKind.CardsView,
            ICardsViewBindingContext => CardsViewBindingContextKind.TypedCustomContext,
            _ => CardsViewBindingContextKind.LegacyCustomContext
        };
}
