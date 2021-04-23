using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework.Services;

namespace AutomotiveDemo.Services
{
    public class InteractionService : Service
    {
        private int colorIndex;
        private bool doorOpened;
        private bool insideCamera;

        public int ColorIndex
        {
            get
            {
                return this.colorIndex;
            }

            set
            {
                this.colorIndex = value;
                this.CarColorChanged?.Invoke(this, value);
            }
        }

        public bool DoorOpened
        {
            get
            {
                return this.doorOpened;
            }

            set
            {
                this.doorOpened = value;
                this.DoorChanged?.Invoke(this, value);
            }
        }

        public bool InsideCamera
        {
            get
            {
                return this.insideCamera;
            }

            set
            {
                this.insideCamera = value;
                this.CameraChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<int> CarColorChanged;

        public event EventHandler<bool> DoorChanged;

        public event EventHandler<bool> CameraChanged;
    }
}
