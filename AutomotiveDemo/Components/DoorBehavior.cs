using AutomotiveDemo.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Evergine.Components.Animation;
using Evergine.Framework;

namespace AutomotiveDemo.Components
{
    public class DoorBehavior : Component
    {
        [BindService]
        InteractionService interaction = null;

        [BindComponent]
        Animation3D carAnimation;

        protected override bool OnAttached()
        {
            this.interaction.DoorChanged += this.OnDoorChanged;
            return base.OnAttached();
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            this.interaction.DoorChanged -= this.OnDoorChanged;
        }

        private void OnDoorChanged(object sender, bool e)
        {
            var startTime = e ? 0 : 0.5f;
            var endTime = e ? 0.5f : 1;

            this.carAnimation.PlayAnimation("Take 001", false, 0, 1, startTime, endTime);
        }
    }
}
