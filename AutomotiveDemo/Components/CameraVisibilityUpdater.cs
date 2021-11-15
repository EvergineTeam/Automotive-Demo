using AutomotiveDemo.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Evergine.Framework;
using Evergine.Framework.Graphics;

namespace AutomotiveDemo.Components
{
    public class CameraVisibilityUpdater : Component
    {
        [BindService]
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

            this.interaction.CameraChanged -= OnCameraChanged;
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
