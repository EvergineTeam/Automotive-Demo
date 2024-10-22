using Evergine.Common.Input.Keyboard;
using Evergine.Common.Input.Mouse;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Mathematics;
using System;

namespace AutomotiveDemo.Components
{
    public class OrbitBehavior : Behavior
    {
        [BindComponent(false)]
        public Transform3D Transform = null;

        [BindComponent(isExactType: false, source: BindComponentSource.ChildrenSkipOwner)]
        private Transform3D targetTransform = null;

        private MouseDispatcher mouseDispatcher;
        private KeyboardDispatcher keyboardDispatcher;

        private float theta;
        private float lambda;
        private float zoom;

        private Quaternion objectOrbitSmoothDampDeriv;
        private float zoomVelocity;

        public float OrbitMouseFactor = 0.0025f;
        public float OrbitKeyboardFactor = 0.01f;
        public float ZoomFactor = 0.5f;
        public float MaxZoom = 0.5f;
        public float MinZoom = 5f;
        public float MaxElevationAngle = 0.5f;
        public float MinElevationAngle = 0.05f;
        public float OrbitSmooth = 50;
        public float ZoomSmooth = 100;

        /// <inheritdoc/>
        protected override bool OnAttached()
        {
            this.theta = -this.Transform.LocalRotation.Y;
            this.lambda = -this.Transform.LocalRotation.X;
            this.zoom = this.targetTransform.LocalPosition.Z;

            return base.OnAttached();
        }

        /// <inheritdoc/>
        protected override void Update(TimeSpan gameTime)
        {
            if (this.mouseDispatcher == null)
            {
                var display = this.Managers.RenderManager.ActiveCamera3D?.Display;

                if (display != null)
                {
                    this.mouseDispatcher = display.MouseDispatcher;
                    this.keyboardDispatcher = display.KeyboardDispatcher;
                }

                if ((this.mouseDispatcher == null) || (this.keyboardDispatcher == null))
                {
                    return;
                }
            }

            if (this.mouseDispatcher.IsButtonDown(MouseButtons.Left) || this.mouseDispatcher.IsButtonDown(MouseButtons.Right))
            {
                var deltaRotation = this.mouseDispatcher.PositionDelta.ToVector2() * this.OrbitMouseFactor;
                this.theta += deltaRotation.X;
                this.lambda += deltaRotation.Y;
                this.lambda = Math.Max(this.MinElevationAngle, Math.Min(this.MaxElevationAngle, this.lambda));
            }

            if (this.keyboardDispatcher.IsKeyDown(Keys.A) || this.keyboardDispatcher.IsKeyDown(Keys.Left))
            {
                this.theta += this.OrbitKeyboardFactor;
            }

            if (this.keyboardDispatcher.IsKeyDown(Keys.D) || this.keyboardDispatcher.IsKeyDown(Keys.Right))
            {
                this.theta -= this.OrbitKeyboardFactor;
            }

            if (this.keyboardDispatcher.IsKeyDown(Keys.W) || this.keyboardDispatcher.IsKeyDown(Keys.Up))
            {
                this.lambda += this.OrbitKeyboardFactor;
                this.lambda = Math.Max(this.MinElevationAngle, Math.Min(this.MaxElevationAngle, this.lambda));
            }

            if (this.keyboardDispatcher.IsKeyDown(Keys.S) || this.keyboardDispatcher.IsKeyDown(Keys.Down))
            {
                this.lambda -= this.OrbitKeyboardFactor;
                this.lambda = Math.Max(this.MinElevationAngle, Math.Min(this.MaxElevationAngle, this.lambda));
            }

            if (this.mouseDispatcher.ScrollDelta.Y != 0)
            {
                var deltazoom = this.mouseDispatcher.ScrollDelta.Y * this.ZoomFactor;

                this.zoom -= deltazoom;

                this.zoom = Math.Max(this.MaxZoom, Math.Min(this.MinZoom, this.zoom));
            }

            float elapsedMilliseconds = (float)gameTime.TotalMilliseconds;

            var direction = Quaternion.CreateFromYawPitchRoll(-this.theta, -this.lambda, 0);

            this.Transform.LocalOrientation = Quaternion.SmoothDamp(
                this.Transform.LocalOrientation,
                direction,
                ref this.objectOrbitSmoothDampDeriv,
                this.OrbitSmooth,
                elapsedMilliseconds);

            var localPos = this.targetTransform.LocalPosition;
            localPos.Z = MathHelper.SmoothDamp(
                localPos.Z,
                this.zoom,
                ref this.zoomVelocity,
                this.ZoomSmooth,
                elapsedMilliseconds);
            this.targetTransform.LocalPosition = localPos;

        }
    }
}
