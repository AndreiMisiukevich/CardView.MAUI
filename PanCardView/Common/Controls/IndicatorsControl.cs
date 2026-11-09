using PanCardView.Behaviors;
using PanCardView.Extensions;
using System.Collections;
using static PanCardView.Controls.Styles.DefaultIndicatorItemStyles;
using PanCardView.Utility;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace PanCardView.Controls
{
    public class IndicatorsControl : HorizontalStackLayout
    {
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(IndicatorsControl), 0, propertyChanged: (bindable, oldValue, newValue) =>
        {
            bindable.AsIndicatorsControl().ResetIndicatorsStyles();
        });

        public static readonly BindableProperty ItemsCountProperty = BindableProperty.Create(nameof(ItemsCount), typeof(int), typeof(IndicatorsControl), -1);

        public static readonly BindableProperty SelectedIndicatorStyleProperty = BindableProperty.Create(nameof(SelectedIndicatorStyle), typeof(Style), typeof(IndicatorsControl), DefaultSelectedIndicatorItemStyle, propertyChanged: (bindable, oldValue, newValue) =>
        {
            bindable.AsIndicatorsControl().ResetIndicatorsStyles();
        });

        public static readonly BindableProperty UnselectedIndicatorStyleProperty = BindableProperty.Create(nameof(UnselectedIndicatorStyle), typeof(Style), typeof(IndicatorsControl), DefaultUnselectedIndicatorItemStyle, propertyChanged: (bindable, oldValue, newValue) =>
        {
            bindable.AsIndicatorsControl().ResetIndicatorsStyles();
        });

        public static readonly BindableProperty IsUserInteractionRunningProperty = BindableProperty.Create(nameof(IsUserInteractionRunning), typeof(bool), typeof(IndicatorsControl), true, propertyChanged: (bindable, oldValue, newValue) =>
        {
            bindable.AsIndicatorsControl().ResetVisibility();
        });

        public static readonly BindableProperty IsAutoInteractionRunningProperty = BindableProperty.Create(nameof(IsAutoInteractionRunning), typeof(bool), typeof(IndicatorsControl), true, propertyChanged: (bindable, oldValue, newValue) =>
        {
            bindable.AsIndicatorsControl().ResetVisibility();
        });

        public static readonly BindableProperty HidesForSingleIndicatorProperty = BindableProperty.Create(nameof(HidesForSingleIndicator), typeof(bool), typeof(IndicatorsControl), true, propertyChanged: (bindable, oldValue, newValue) =>
        {
            bindable.AsIndicatorsControl().ResetVisibility();
        });

        public static readonly BindableProperty MaximumVisibleIndicatorsCountProperty = BindableProperty.Create(nameof(MaximumVisibleIndicatorsCount), typeof(int), typeof(IndicatorsControl), int.MaxValue, propertyChanged: (bindable, oldValue, newValue) =>
        {
            bindable.AsIndicatorsControl().ResetVisibility();
        });

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(IndicatorsControl), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            bindable.AsIndicatorsControl().ResetItemsSource(oldValue as IEnumerable);
        });

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(IndicatorsControl), new DataTemplate(() => new IndicatorItemView()), propertyChanged: (bindable, oldValue, newValue) =>
        {
            bindable.AsIndicatorsControl().ResetIndicatorsLayout();
        });

        public static readonly BindableProperty UseParentAsBindingContextProperty = BindableProperty.Create(nameof(UseParentAsBindingContext), typeof(bool), typeof(IndicatorsControl), true);

        public static readonly BindableProperty IsTapToNavigateEnabledProperty = BindableProperty.Create(nameof(IsTapToNavigateEnabled), typeof(bool), typeof(IndicatorsControl), true);

        public static readonly BindableProperty ToFadeDurationProperty = BindableProperty.Create(nameof(ToFadeDuration), typeof(int), typeof(IndicatorsControl), 0);

        static IndicatorsControl()
        {
        }

        private CancellationTokenSource _fadeAnimationTokenSource;
        private CardsViewBindingContextKind? _bindingContextKind;
        private ControlBindingManager _bindingManager;

        public IndicatorsControl()
        {
            Spacing = 5;

            Margin = new Thickness(10, 20);
            VerticalOptions = LayoutOptions.End;
            HorizontalOptions = LayoutOptions.Center;

            Behaviors.Add(new ProtectedControlBehavior());
        }

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public int ItemsCount
        {
            get => (int)GetValue(ItemsCountProperty);
            set => SetValue(ItemsCountProperty, value);
        }

        public Style SelectedIndicatorStyle
        {
            get => GetValue(SelectedIndicatorStyleProperty) as Style;
            set => SetValue(SelectedIndicatorStyleProperty, value);
        }

        public Style UnselectedIndicatorStyle
        {
            get => GetValue(UnselectedIndicatorStyleProperty) as Style;
            set => SetValue(UnselectedIndicatorStyleProperty, value);
        }

        public bool IsUserInteractionRunning
        {
            get => (bool)GetValue(IsUserInteractionRunningProperty);
            set => SetValue(IsUserInteractionRunningProperty, value);
        }

        public bool IsAutoInteractionRunning
        {
            get => (bool)GetValue(IsAutoInteractionRunningProperty);
            set => SetValue(IsAutoInteractionRunningProperty, value);
        }

        public bool HidesForSingleIndicator
        {
            get => (bool)GetValue(HidesForSingleIndicatorProperty);
            set => SetValue(HidesForSingleIndicatorProperty, value);
        }

        public int MaximumVisibleIndicatorsCount
        {
            get => (int)GetValue(MaximumVisibleIndicatorsCountProperty);
            set => SetValue(MaximumVisibleIndicatorsCountProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => GetValue(ItemsSourceProperty) as IEnumerable;
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => GetValue(ItemTemplateProperty) as DataTemplate;
            set => SetValue(ItemTemplateProperty, value);
        }

        public bool UseParentAsBindingContext
        {
            get => (bool)GetValue(UseParentAsBindingContextProperty);
            set => SetValue(UseParentAsBindingContextProperty, value);
        }

        public bool IsTapToNavigateEnabled
        {
            get => (bool)GetValue(IsTapToNavigateEnabledProperty);
            set => SetValue(IsTapToNavigateEnabledProperty, value);
        }

        public int ToFadeDuration
        {
            get => (int)GetValue(ToFadeDurationProperty);
            set => SetValue(ToFadeDurationProperty, value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void Preserve()
        {
        }

        public object GetItemByIndex(int index) => ItemsSource?.FindValue(index);

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (UseParentAsBindingContext && Parent is CardsView)
            {
                BindingContext = Parent;
            }

            UpdateBindings();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateBindings();
            ResetIndicatorsLayout();
        }

        private void UpdateBindings()
        {
            var bindingContextKind = this.GetCardsViewBindingContextKind();
            if (_bindingContextKind == bindingContextKind)
            {
                return;
            }

            _bindingContextKind = bindingContextKind;

            switch (bindingContextKind)
            {
                case CardsViewBindingContextKind.CardsView:
                    BindToCardsView();
                    break;
                case CardsViewBindingContextKind.TypedCustomContext:
                    BindToTypedCustomContext();
                    break;
                default:
                    BindToLegacyCustomContext();
                    break;
            }
        }

        private void BindToCardsView()
        {
            SetDefaultBinding(SelectedIndexProperty, () => this.SetBinding(SelectedIndexProperty, static (CardsView source) => source.SelectedIndex, BindingMode.OneWay));
            SetDefaultBinding(ItemsSourceProperty, () => this.SetBinding(ItemsSourceProperty, static (CardsView source) => source.ItemsSource, BindingMode.OneWay));
            SetDefaultBinding(IsUserInteractionRunningProperty, () => this.SetBinding(IsUserInteractionRunningProperty, static (CardsView source) => source.IsUserInteractionRunning, BindingMode.OneWay));
            SetDefaultBinding(IsAutoInteractionRunningProperty, () => this.SetBinding(IsAutoInteractionRunningProperty, static (CardsView source) => source.IsAutoInteractionRunning, BindingMode.OneWay));
        }

        private void BindToTypedCustomContext()
        {
            SetDefaultBinding(SelectedIndexProperty, () => this.SetBinding(SelectedIndexProperty, static (ICardsViewBindingContext source) => source.SelectedIndex, BindingMode.OneWay));
            SetDefaultBinding(ItemsSourceProperty, () => this.SetBinding(ItemsSourceProperty, static (ICardsViewBindingContext source) => source.ItemsSource, BindingMode.OneWay));
            SetDefaultBinding(IsUserInteractionRunningProperty, () => this.SetBinding(IsUserInteractionRunningProperty, static (ICardsViewBindingContext source) => source.IsUserInteractionRunning, BindingMode.OneWay));
            SetDefaultBinding(IsAutoInteractionRunningProperty, () => this.SetBinding(IsAutoInteractionRunningProperty, static (ICardsViewBindingContext source) => source.IsAutoInteractionRunning, BindingMode.OneWay));
        }

        [RequiresUnreferencedCode("Uses legacy string-path bindings for custom view models. Preserve their bound properties or implement ICardsViewBindingContext for trimmed applications.")]
        private void BindToLegacyCustomContext()
        {
            SetDefaultBinding(SelectedIndexProperty, () => this.SetBinding(SelectedIndexProperty, nameof(CardsView.SelectedIndex), BindingMode.OneWay));
            SetDefaultBinding(ItemsSourceProperty, () => this.SetBinding(ItemsSourceProperty, nameof(CardsView.ItemsSource), BindingMode.OneWay));
            SetDefaultBinding(IsUserInteractionRunningProperty, () => this.SetBinding(IsUserInteractionRunningProperty, nameof(CardsView.IsUserInteractionRunning), BindingMode.OneWay));
            SetDefaultBinding(IsAutoInteractionRunningProperty, () => this.SetBinding(IsAutoInteractionRunningProperty, nameof(CardsView.IsAutoInteractionRunning), BindingMode.OneWay));
        }

        private void SetDefaultBinding(BindableProperty property, Action setBinding)
            => BindingManager.SetDefaultBinding(property, setBinding);

        private ControlBindingManager BindingManager => _bindingManager ??= new(this);

        private void SetSelectedIndexBinding(BindingMode mode)
        {
            switch (this.GetCardsViewBindingContextKind())
            {
                case CardsViewBindingContextKind.CardsView:
                    BindingManager.TryReplaceOwnedBinding(SelectedIndexProperty, () => this.SetBinding(SelectedIndexProperty, static (CardsView source) => source.SelectedIndex, mode));
                    break;
                case CardsViewBindingContextKind.TypedCustomContext:
                    BindingManager.TryReplaceOwnedBinding(SelectedIndexProperty, () => this.SetBinding(SelectedIndexProperty, static (ICardsViewBindingContext source) => source.SelectedIndex, mode));
                    break;
                default:
                    SetLegacySelectedIndexBinding(mode);
                    break;
            }
        }

        [RequiresUnreferencedCode("Uses a legacy string-path binding for custom view models. Preserve their SelectedIndex property or implement ICardsViewBindingContext for trimmed applications.")]
        private void SetLegacySelectedIndexBinding(BindingMode mode)
            => BindingManager.TryReplaceOwnedBinding(SelectedIndexProperty, () => this.SetBinding(SelectedIndexProperty, nameof(CardsView.SelectedIndex), mode));

        protected virtual void ApplySelectedStyle(View view, int index)
        => view.Style = SelectedIndicatorStyle;

        protected virtual void ApplyUnselectedStyle(View view, int index)
        => view.Style = UnselectedIndicatorStyle;

        protected virtual int IndexOf(View view) => Children.IndexOf(view);

        protected virtual void OnResetIndicatorsStyles(int currentIndex)
        {
            foreach (var child in Children)
            {
                ApplyStyle(child as View, currentIndex);
            }
        }

        protected virtual async void ResetVisibility(uint? appearingTime = null, Easing appearingEasing = null, uint? dissappearingTime = null, Easing disappearingEasing = null)
        {
            _fadeAnimationTokenSource?.Cancel();

            if (ItemsCount > MaximumVisibleIndicatorsCount ||
                (HidesForSingleIndicator && ItemsCount <= 1 && ItemsCount >= 0))
            {
                Opacity = 0;
                IsVisible = false;
                return;
            }

            if(ToFadeDuration <= 0)
            {
                Opacity = 1;
                IsVisible = true;
                return;
            }

            if (IsUserInteractionRunning || IsAutoInteractionRunning)
            {
                IsVisible = true;

                await new AnimationWrapper(v => Opacity = v, Opacity, 1)
                    .Commit(this, nameof(ResetVisibility), 16, appearingTime ?? 330, appearingEasing ?? Easing.CubicInOut);
                return;
            }

            _fadeAnimationTokenSource = new CancellationTokenSource();
            var token = _fadeAnimationTokenSource.Token;

            await Task.Delay(ToFadeDuration);
            if (token.IsCancellationRequested)
            {
                return;
            }

            await new AnimationWrapper(v => Opacity = v, Opacity, 0)
                .Commit(this, nameof(ResetVisibility), 16, dissappearingTime ?? 330, disappearingEasing ?? Easing.SinOut);

            if (token.IsCancellationRequested)
            {
                return;
            }
            IsVisible = false;
        }

        private void ResetItemsSource(IEnumerable oldCollection)
        {
            if (oldCollection is INotifyCollectionChanged oldObservableCollection)
            {
                oldObservableCollection.CollectionChanged -= OnObservableCollectionChanged;
            }

            if (ItemsSource is INotifyCollectionChanged observableCollection)
            {
                observableCollection.CollectionChanged += OnObservableCollectionChanged;
            }

            OnObservableCollectionChanged(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        => ResetIndicatorsLayout();

        private void ApplyStyle(View view, int selectedIndex)
        {
            try
            {
                view.BatchBegin();
                var index = IndexOf(view);
                if (index == selectedIndex)
                {
                    ApplySelectedStyle(view, index);
                    return;
                }
                ApplyUnselectedStyle(view, index);
            }
            finally
            {
                view.BatchCommit();
            }
        }

        private void ResetIndicatorsStylesNonBatch()
        {
            var cyclicalIndex = SelectedIndex.ToCyclicalIndex(ItemsCount);
            OnResetIndicatorsStyles(cyclicalIndex);
        }

        private void ResetIndicatorsStyles()
        {
            try
            {
                BatchBegin();
                ResetIndicatorsStylesNonBatch();
            }
            finally
            {
                BatchCommit();
            }
        }

        private void ResetIndicatorsLayout()
        {
            try
            {
                BatchBegin();
                Children.Clear();
                if (ItemsSource == null)
                {
                    return;
                }

                ItemsCount = ItemsSource.Count();
                foreach(var item in ItemsSource)
                {
                    var view = ItemTemplate?.SelectTemplate(item)?.CreateView() ?? item as View;
                    if (view == null)
                    {
                        return;
                    }

                    if (!Equals(view, item))
                    {
                        view.BindingContext = item;
                    }

                    view.GestureRecognizers.Clear();
                    view.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        CommandParameter = item,
                        Command = new Command(p =>
                        {
                            if (!IsTapToNavigateEnabled)
                            {
                                return;
                            }
                            SetSelectedIndexBinding(BindingMode.OneWayToSource);
                            SelectedIndex = ItemsSource.FindIndex(p);
                            SetSelectedIndexBinding(BindingMode.OneWay);
                        })
                    });
                    Children.Add(view);
                }
            }
            finally
            {
                ResetIndicatorsStylesNonBatch();
                ResetVisibility();
                BatchCommit();
            }
        }
    }
}
