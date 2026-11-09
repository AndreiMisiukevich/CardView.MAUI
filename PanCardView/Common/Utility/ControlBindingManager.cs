using Microsoft.Maui.Controls;

namespace PanCardView.Utility
{
    /// <summary>
    /// Installs defaults only for properties that were not set by the application,
    /// then tracks those defaults for binding-context changes.
    /// </summary>
    internal sealed class ControlBindingManager
    {
        private readonly BindableObject _bindable;
        private readonly HashSet<BindableProperty> _installedDefaults = new();

        public ControlBindingManager(BindableObject bindable)
        {
            _bindable = bindable;
        }

        public void SetDefaultBinding(BindableProperty property, Action setBinding)
        {
            if (!_installedDefaults.Contains(property) && _bindable.IsSet(property))
            {
                return;
            }

            setBinding();
            _installedDefaults.Add(property);
        }

        public bool TryReplaceOwnedBinding(BindableProperty property, Action setBinding)
        {
            if (!_installedDefaults.Contains(property))
            {
                return false;
            }

            setBinding();
            return true;
        }
    }
}
