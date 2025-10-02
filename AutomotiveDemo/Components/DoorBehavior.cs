using AutomotiveDemo.Services;
using Evergine.Components.Animation;
using Evergine.Framework;

namespace AutomotiveDemo.Components
{
    public class DoorBehavior : Component
    {
        [BindService(isRequired: false)]
        InteractionService interaction = null;

        [BindComponent]
        Animation3D carAnimation = null;

        protected override bool OnAttached()
        {
            if (!Application.Current.IsEditor)
            {
                this.interaction.DoorChanged += this.OnDoorChanged;
            }

            return base.OnAttached();
        }

        protected override void OnDetached()
        {
            base.OnDetached();

            if (!Application.Current.IsEditor)
            {
                this.interaction.DoorChanged -= this.OnDoorChanged;
            }
        }

        private void OnDoorChanged(object sender, bool e)
        {
            var startTime = e ? 0 : 0.5f;
            var endTime = e ? 0.5f : 1;

            this.carAnimation.PlayAnimation("Take 001", false, 0, 1, startTime, endTime);
        }
    }
}
