using Android.Content;
using Android.Views;
using JetBrains.Annotations;
using static System.Math;
using PanCardView.Enums;
using XView = Microsoft.Maui.Controls.View;
using Microsoft.Maui.Platform;
using View = Android.Views.View;

namespace PanCardView;

public partial class CardsViewHandler
{
    private const int SwipeThreshold = 100;
    private const int SwipeVelocityThreshold = 1200;
    private CardsView Element => VirtualView as CardsView;
    
    protected override LayoutViewGroup CreatePlatformView()
    {
        var viewGroup = new CardsViewLayoutViewGroup(Context!)
        {
            CrossPlatformLayout = VirtualView
        };
        
        viewGroup.SetClipChildren(false);

        return viewGroup;
    }
    
    protected override void ConnectHandler(LayoutViewGroup platformView)
    {
        base.ConnectHandler(platformView);

        (platformView as CardsViewLayoutViewGroup).PanStarted = false; 
        Element.AccessibilityChangeRequested += OnAccessibilityChangeRequested;
    }
    
    protected override void DisconnectHandler(LayoutViewGroup platformView)
    {
        base.DisconnectHandler(platformView);

        Element.AccessibilityChangeRequested -= OnAccessibilityChangeRequested;
    }
    
    private void OnAccessibilityChangeRequested(object sender, bool isEnabled)
    {
        if (sender is XView view)
        {
            if (view.Handler?.PlatformView is View nativeView)
            {
                nativeView.ImportantForAccessibility = isEnabled
                    ? ImportantForAccessibility.Auto
                    : ImportantForAccessibility.NoHideDescendants;
            }
        }
    }

    private sealed class CardsViewLayoutViewGroup : LayoutViewGroup
    {
        private float? _startX;
        private float? _startY;
        private GestureDetector _gestureDetector;

        private CardsView Element => CrossPlatformLayout as CardsView;

        public bool PanStarted;

        public CardsViewLayoutViewGroup([NotNull] Context context) : base(context)
        {
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            switch (e.ActionMasked)
            {
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    if (Abs(Element?.CurrentDiff ?? 0) <= 0)
                    {
                        HandleUpCancelEvent(GestureStatus.Canceled, 0, 0);
                    }
                    break;
            }
            return base.DispatchTouchEvent(e);
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            DetectEvent(ev);

            if (Element.IsPanControllingByChild || !Element.IsPanInteractionEnabled || Element.ShouldThrottlePanInteraction)
            {
                base.OnInterceptTouchEvent(ev);
                return false;
            }

            if (ev.ActionMasked == MotionEventActions.Move)
            {
                return CheckTouchHandled(GetTotalX(ev), GetTotalY(ev));
            }

            HandleDownUpEvents(ev);
            return false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            DetectEvent(e);
            if (e.ActionMasked == MotionEventActions.Move)
            {
                var xDelta = GetTotalX(e);
                var yDelta = GetTotalY(e);
                CheckTouchHandled(xDelta, yDelta);

                UpdatePan(GestureStatus.Running, xDelta, yDelta);
            }

            HandleDownUpEvents(e);
            return true;
        }
        
        private void DetectEvent(MotionEvent ev)
        {
            if (ev.PointerCount > 1)
            {
                return;
            }
            try
            {
                if (_gestureDetector == null)
                {
                    SetGestureDetector();
                }
                _gestureDetector.OnTouchEvent(ev);
            }
            catch (ObjectDisposedException)
            {
                SetGestureDetector();
                DetectEvent(ev);
            }
        }

        private bool CheckTouchHandled(float xDelta, float yDelta)
        {
            var xDeltaAbs = Abs(xDelta);
            var yDeltaAbs = Abs(yDelta);
            var isHandled = Element.IsHorizontalOrientation
                ? (yDeltaAbs > xDeltaAbs &&
                             Element.IsVerticalSwipeEnabled &&
                             yDeltaAbs > Element.VerticalSwipeThresholdDistance) ||
                            (xDeltaAbs > yDeltaAbs &&
                             xDeltaAbs > Element.MoveThresholdDistance)
                : (xDeltaAbs > yDeltaAbs &&
                             Element.IsPanSwipeEnabled &&
                             xDeltaAbs > Element.SwipeThresholdDistance) ||
                            (yDeltaAbs > xDeltaAbs &&
                             yDeltaAbs > Element.MoveThresholdDistance);

            if (!Element.IsPanInteractionEnabled || !Element.IsUserInteractionEnabled)
                isHandled = false;

            Element.IsUserInteractionRunning |= isHandled;
            Parent?.RequestDisallowInterceptTouchEvent(isHandled);
            return isHandled;
        }

        private void HandleDownUpEvents(MotionEvent ev)
        {
            HandleDownEvent(ev);
            HandleUpCancelEvent(ev);
        }

        private void HandleUpCancelEvent(MotionEvent ev)
        {
            var action = ev.ActionMasked;
            var isUpAction = action == MotionEventActions.Up;
            var isCancelAction = action == MotionEventActions.Cancel;

            if (!isUpAction && !isCancelAction)
            {
                return;
            }

            var xDelta = GetTotalX(ev);
            var yDelta = GetTotalY(ev);
            HandleUpCancelEvent(isUpAction ? GestureStatus.Completed : GestureStatus.Canceled, xDelta, yDelta);
        }

        private void HandleDownEvent(MotionEvent ev)
        {
            if (ev.ActionMasked != MotionEventActions.Down)
            {
                return;
            }
            _startX = ev.GetX();
            _startY = ev.GetY();

            UpdatePan(GestureStatus.Started);
            PanStarted = true;
        }

        private void HandleUpCancelEvent(GestureStatus status, double xDelta, double yDelta)
        {
            if (!PanStarted)
            {
                return;
            }
            PanStarted = false;

            UpdatePan(status, xDelta, yDelta);

            Parent?.RequestDisallowInterceptTouchEvent(false);

            _startX = null;
            _startY = null;
        }

        private void UpdatePan(GestureStatus status, double totalX = 0, double totalY = 0)
        => Element.OnPanUpdated(GetPanUpdatedEventArgs(status, totalX, totalY));

        private void OnSwiped(ItemSwipeDirection swipeDirection) => Element.OnSwiped(swipeDirection);

        private PanUpdatedEventArgs GetPanUpdatedEventArgs(GestureStatus status, double totalX = 0, double totalY = 0)
        => new PanUpdatedEventArgs(status, 0, totalX, totalY);

        private float GetTotalX(MotionEvent ev) => (ev.GetX() - _startX.GetValueOrDefault()) / Context.Resources.DisplayMetrics.Density;

        private float GetTotalY(MotionEvent ev) => (ev.GetY() - _startY.GetValueOrDefault()) / Context.Resources.DisplayMetrics.Density;

        private void SetGestureDetector()
        => _gestureDetector = new GestureDetector(Context, new CardsGestureListener(OnSwiped));
    }

    private sealed class CardsGestureListener : GestureDetector.SimpleOnGestureListener
    {
        private readonly Action<ItemSwipeDirection> _onSwiped;

        public CardsGestureListener(Action<ItemSwipeDirection> onSwiped)
        => _onSwiped = onSwiped;

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            var diffX = (e2?.GetX() ?? 0) - (e1?.GetX() ?? 0);
            var diffY = (e2?.GetY() ?? 0) - (e1?.GetY() ?? 0);

            var absDiffX = Abs(diffX);
            var absDiffY = Abs(diffY);

            if (absDiffX > absDiffY &&
                absDiffX > SwipeThreshold &&
                Abs(velocityX) > SwipeVelocityThreshold)
            {
                _onSwiped?.Invoke(diffX < 0 ? ItemSwipeDirection.Left : ItemSwipeDirection.Right);
                return true;
            }

            if (absDiffY >= absDiffX &&
               absDiffY > SwipeThreshold &&
               Abs(velocityY) > SwipeVelocityThreshold)
            {
                _onSwiped?.Invoke(diffY < 0 ? ItemSwipeDirection.Up : ItemSwipeDirection.Down);
                return true;
            }

            return false;
        }
    }
}