using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework.Services;

namespace AutomotiveDemo.Services
{
    public class InteractionService : Service
    {
        private Color carColor;

        public Color CarColor
        {
            get
            {
                return this.carColor;
            }

            set
            {
                this.carColor = value;
                this.CarColorChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<Color> CarColorChanged;
    }
}
