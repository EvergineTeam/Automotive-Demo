using AutomotiveDemo.Services;
using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Graphics.Materials;

namespace AutomotiveDemo.Components
{
    public class ColorUpdater : Component
    {
        [BindService]
        InteractionService interaction = null;

        private StandardMaterial targetStandardMaterial;

        protected override bool OnAttached()
        {
            var material = this.Managers.AssetSceneManager.Load<Material>(WaveContent.Models.car_Embedded.Materials.CarPaint);
            this.targetStandardMaterial = new StandardMaterial(material);

            this.interaction.CarColorChanged += this.OnColorChanged;

            return base.OnAttached();
        }

        private void OnColorChanged(object sender, Color newColor)
        {
            this.targetStandardMaterial.BaseColor = newColor;
        }
    }
}
