using System.Collections;

namespace PanCardView;

/// <summary>
/// Describes the binding properties used by CardsView's built-in indicators, arrows, and tabs.
/// Implement this interface to keep custom binding contexts statically analyzable with trimming and NativeAOT.
/// Implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> to propagate property updates to those controls.
/// </summary>
public interface ICardsViewBindingContext
{
    IEnumerable ItemsSource { get; }

    int ItemsCount { get; }

    int SelectedIndex { get; set; }

    bool IsCyclical { get; }

    bool IsUserInteractionRunning { get; }

    bool IsAutoInteractionRunning { get; }

    bool IsRightToLeftFlowDirectionEnabled { get; }

    bool? ShouldAutoNavigateToNext { get; set; }

    double ProcessorDiff { get; }

    double Width { get; }
}
