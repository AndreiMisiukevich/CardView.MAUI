using UIKit;
using PanCardView.Enums;
using Microsoft.Maui.Handlers;
using static System.Math;

using Microsoft.Maui.Platform;

namespace PanCardView;

public partial class CardsViewHandler
{
    private CardsView Element => VirtualView as CardsView;
    
    protected override LayoutView CreatePlatformView() =>
        new CardsViewLayoutView
        {
            CrossPlatformLayout = VirtualView
        };

    protected override void ConnectHandler(LayoutView platformView)
    {
        base.ConnectHandler(platformView);
        
        (platformView as CardsViewLayoutView)?.SetSwipeGestures();
        Element.AccessibilityChangeRequested += OnAccessibilityChangeRequested;
    }

    protected override void DisconnectHandler(LayoutView platformView)
    {
        base.DisconnectHandler(platformView);
        Element.AccessibilityChangeRequested -= OnAccessibilityChangeRequested;
    }
    
    private static void MapIsVerticalSwipeEnabled(LayoutHandler handler, CardsView cardsView)
    {
        (handler.PlatformView as CardsViewLayoutView)?.SetSwipeGestures();
    }

    private static void MapIsUserInteractionEnabled(LayoutHandler handler, CardsView cardsView)
    {
        (handler.PlatformView as CardsViewLayoutView)?.SetSwipeGestures();
    }
    
    private void OnAccessibilityChangeRequested(object sender, bool isEnabled)
    {
        if (sender is View view)
        {
            if (view.Handler?.PlatformView is UIView nativeView)
            {
                nativeView.AccessibilityElementsHidden = !isEnabled;
            }
        }
    }

    private sealed class CardsViewLayoutView : LayoutView
    {
        private UISwipeGestureRecognizer _leftSwipeGesture;
        private UISwipeGestureRecognizer _rightSwipeGesture;
        private UISwipeGestureRecognizer _upSwipeGesture;
        private UISwipeGestureRecognizer _downSwipeGesture;
        
        private CardsView Element => CrossPlatformLayout as CardsView;

        public CardsViewLayoutView()
        {
            _leftSwipeGesture = new UISwipeGestureRecognizer(OnSwiped)
            {
                Direction = UISwipeGestureRecognizerDirection.Left
            };
            _rightSwipeGesture = new UISwipeGestureRecognizer(OnSwiped)
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };
            _upSwipeGesture = new UISwipeGestureRecognizer(OnSwiped)
            {
                Direction = UISwipeGestureRecognizerDirection.Up
            };
            _downSwipeGesture = new UISwipeGestureRecognizer(OnSwiped)
            {
                Direction = UISwipeGestureRecognizerDirection.Down
            };
        }

        public override void AddGestureRecognizer(UIGestureRecognizer gestureRecognizer)
        {
            base.AddGestureRecognizer(gestureRecognizer);

            if (gestureRecognizer is UIPanGestureRecognizer panGestureRecognizer)
            {
                gestureRecognizer.ShouldRecognizeSimultaneously = ShouldRecognizeSimultaneously;
                gestureRecognizer.ShouldBegin = ShouldBegin;
            }
        }

        public void SetSwipeGestures()
        {
            var shouldRemoveAllSwipes = !(Element?.IsUserInteractionEnabled ?? false);
            var shouldRemoveVerticalSwipes = !(Element?.IsVerticalSwipeEnabled ?? false);
        
            ResetSwipeGestureRecognizer(_leftSwipeGesture, shouldRemoveAllSwipes);
            ResetSwipeGestureRecognizer(_rightSwipeGesture, shouldRemoveAllSwipes);
            ResetSwipeGestureRecognizer(_upSwipeGesture, shouldRemoveAllSwipes || shouldRemoveVerticalSwipes);
            ResetSwipeGestureRecognizer(_downSwipeGesture, shouldRemoveAllSwipes || shouldRemoveVerticalSwipes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _leftSwipeGesture?.Dispose();
                _rightSwipeGesture?.Dispose();
                _upSwipeGesture?.Dispose();
                _downSwipeGesture?.Dispose();
                _leftSwipeGesture = null;
                _rightSwipeGesture = null;
                _upSwipeGesture = null;
                _downSwipeGesture = null;
            }
            base.Dispose(disposing);
        }

        private void ResetSwipeGestureRecognizer(UISwipeGestureRecognizer swipeGestureRecognizer, bool isForceRemove = false)
        {
            RemoveGestureRecognizer(swipeGestureRecognizer);
            if (isForceRemove)
            {
                return;
            }
            AddGestureRecognizer(swipeGestureRecognizer);
        }

        private void OnSwiped(UISwipeGestureRecognizer gesture)
        {
            var swipeDirection = gesture.Direction == UISwipeGestureRecognizerDirection.Left
                ? ItemSwipeDirection.Left
                : gesture.Direction == UISwipeGestureRecognizerDirection.Right
                    ? ItemSwipeDirection.Right
                    : gesture.Direction == UISwipeGestureRecognizerDirection.Up
                        ? ItemSwipeDirection.Up
                        : ItemSwipeDirection.Down;

            Element?.OnSwiped(swipeDirection);
        }

        private bool ShouldBegin(UIGestureRecognizer recognizer)
        {
            if (recognizer is UIPanGestureRecognizer pangesture)
            {
                var superview = pangesture.View.Superview;
                while (superview != null)
                {
                    if (superview is UIScrollView)
                    {
                        var velocity = pangesture.VelocityInView(this);
                        var absVelocityX = Abs(velocity.X);
                        var absVelocityY = Abs(velocity.Y);
                        var isHorizontal = Element.IsHorizontalOrientation;
                        return (absVelocityY < absVelocityX && isHorizontal) ||
                               (absVelocityY > absVelocityX && !isHorizontal);
                    }
                    superview = superview.Superview;
                }
            }

            return true;
        }

        private bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
            => Element == null || !(otherGestureRecognizer is UIPanGestureRecognizer) || otherGestureRecognizer.View is CardsViewLayoutView;
    }
}