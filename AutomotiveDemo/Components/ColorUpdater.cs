using AutomotiveDemo.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Evergine.Common.Graphics;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Framework.Graphics.Materials;

namespace AutomotiveDemo.Components
{
    public class ColorUpdater : Component
    {
        [BindService]
        InteractionService interaction = null;

        private StandardMaterial targetStandardMaterial;

        protected override bool OnAttached()
        {
            var material = this.Managers.AssetSceneManager.Load<Material>(EvergineContent.Materials.Car.CarPaint);
            this.targetStandardMaterial = new StandardMaterial(material);

            this.interaction.CarColorChanged += this.OnColorChanged;

            return base.OnAttached();
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            this.interaction.CarColorChanged -= this.OnColorChanged;
        }

        private void OnColorChanged(object sender, int colorIndex)
        {
            switch (colorIndex)
            {
                case 0:
                default:
                    this.targetStandardMaterial.BaseColor = Color.White;
                    this.targetStandardMaterial.Metallic = 0.1f;
                    this.targetStandardMaterial.Roughness = 0.4f;
                    break;
                case 1:
                    this.targetStandardMaterial.BaseColor = Color.LightGray;
                    this.targetStandardMaterial.Metallic = 0.8f;
                    this.targetStandardMaterial.Roughness = 0.4f;
                    break;
                case 2:
                    this.targetStandardMaterial.BaseColor = Color.Gray;
                    this.targetStandardMaterial.Metallic = 0.8f;
                    this.targetStandardMaterial.Roughness = 0.4f;
                    break;
                case 3:
                    this.targetStandardMaterial.BaseColor = Color.DodgerBlue;
                    this.targetStandardMaterial.Metallic = 0.4f;
                    this.targetStandardMaterial.Roughness= 0.3f;
                    break;
                case 4:
                    this.targetStandardMaterial.BaseColor = Color.Orange;
                    this.targetStandardMaterial.Metallic = 0.4f;
                    this.targetStandardMaterial.Roughness = 0.4f;
                    break;
                case 5:
                    this.targetStandardMaterial.BaseColor = Color.Red;
                    this.targetStandardMaterial.Metallic = 0.2f;
                    this.targetStandardMaterial.Roughness = 0.3f;
                    break;
                case 6:
                    this.targetStandardMaterial.BaseColor = Color.Black;
                    this.targetStandardMaterial.Metallic = 0.1f;
                    this.targetStandardMaterial.Roughness = 0.4f;
                    break;
            }
        }
    }
}
