using AutomotiveDemo.Services;
using Evergine.Framework;

namespace AutomotiveDemo.Components
{
    public class CameraVisibilityUpdater : Component
    {
        [BindService(isRequired: false)]
        InteractionService interaction = null;

        public bool IsInside;

        protected override bool OnAttached()
        {
            if (!Application.Current.IsEditor)
            {
                this.RefreshCameraVisibility();
                this.interaction.CameraChanged += OnCameraChanged;
            }

            return base.OnAttached();
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (!Application.Current.IsEditor)
            {
                this.interaction.CameraChanged -= OnCameraChanged;
            }
        }

        private void RefreshCameraVisibility()
        {
            this.Owner.IsEnabled = this.IsInside ? this.interaction.InsideCamera : !this.interaction.InsideCamera;
        }

        private void OnCameraChanged(object sender, bool e)
        {
            this.RefreshCameraVisibility();
        }
    }
}
