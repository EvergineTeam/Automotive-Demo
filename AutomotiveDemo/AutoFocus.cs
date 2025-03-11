using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Mathematics;
using System;
using System.Runtime.CompilerServices;

namespace AutomotiveDemo
{
    public class AutoFocus : Behavior
    {
        [BindComponent(isExactType: false)]
        private Camera camera = null;

        private Transform3D targetTransform;
        private string target = "FocusTarget";

        public string Target
        {
            get
            {
                return this.target;
            }

            set
            {
                this.target = value;

                if (this.IsAttached)
                {
                    this.RefreshTarget();
                }
            }
        }

        public float Smooth = 0.4f;

        public bool ActiveCamera = false;

        private float currentVelocity;

        protected override bool OnAttached()
        {
            this.Family = FamilyType.PriorityBehavior;
            this.RefreshTarget();
            return base.OnAttached();
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (this.targetTransform != null)
            {
                var usedCamera = this.ActiveCamera ? this.Managers.RenderManager.ActiveCamera3D : this.camera;

                var distance = Vector3.Distance(this.targetTransform.Position, usedCamera.Position);

                usedCamera.FocalDistance = MathHelper.SmoothDamp(usedCamera.FocalDistance, distance, ref this.currentVelocity, this.Smooth, (float)gameTime.TotalSeconds);
            }
        }

        private void RefreshTarget()
        {
            this.targetTransform = !string.IsNullOrEmpty(this.target) ? this.Managers.EntityManager.FindFirstComponentOfType<Transform3D>(tag: this.target) : null;
        }
    }
}
