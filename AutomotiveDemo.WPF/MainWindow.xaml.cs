using AutomotiveDemo.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WaveEngine.Common.Graphics;
using WaveEngine.DirectX11;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.WPF;
using Window = System.Windows.Window;

namespace AutomotiveDemo.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DX11GraphicsContext dX11GraphicsContext;
        private WPFSurface surface;
        private Display display;
        private InteractionService interactionService;

        private int currentScreen = 0;

        public MainWindow()
        {
            InitializeComponent();
            LoadWaveEngineControl();
        }

        private void LoadWaveEngineControl()
        {
            var application = ((App)Application.Current).WaveApplication;

            var graphicsPresenter = application.Container.Resolve<GraphicsPresenter>();
            dX11GraphicsContext = application.Container.Resolve<DX11GraphicsContext>();

            surface = new WPFSurface(0, 0) { SurfaceUpdatedAction = SurfaceUpdated };
            display = new Display(surface, (FrameBuffer)null);

            WaveContainer.Content = surface.NativeControl;
            surface.NativeControl.MouseDown += NativeControlMouseDown;
            graphicsPresenter.AddDisplay("DefaultDisplay", display);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.Key == Key.F)
            {
                var toFullScreen = this.WindowState != WindowState.Maximized;
                this.WindowState = toFullScreen ? WindowState.Maximized: WindowState.Normal;
                this.WindowStyle = toFullScreen ? WindowStyle.None : WindowStyle.SingleBorderWindow;
            }
        }

        private void NativeControlMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).Focus();
        }

        private void SurfaceUpdated(IntPtr surfaceHandle)
        {
            SharpDX.ComObject sharedObject = new SharpDX.ComObject(surfaceHandle);
            SharpDX.DXGI.Resource sharedResource = sharedObject.QueryInterface<SharpDX.DXGI.Resource>();
            SharpDX.Direct3D11.Texture2D nativeRexture = dX11GraphicsContext.DXDevice.OpenSharedResource<SharpDX.Direct3D11.Texture2D>(sharedResource.SharedHandle);

            var texture = DX11Texture.FromDirectXTexture(dX11GraphicsContext, nativeRexture);
            var rTDepthTargetDescription = new TextureDescription()
            {
                Type = TextureType.Texture2D,
                Format = WaveEngine.Common.Graphics.PixelFormat.D24_UNorm_S8_UInt,
                Width = texture.Description.Width,
                Height = texture.Description.Height,
                Depth = 1,
                ArraySize = 1,
                Faces = 1,
                Flags = TextureFlags.DepthStencil,
                CpuAccess = ResourceCpuAccess.None,
                MipLevels = 1,
                Usage = ResourceUsage.Default,
                SampleCount = TextureSampleCount.None,
            };

            var rTDepthTarget = this.dX11GraphicsContext.Factory.CreateTexture(ref rTDepthTargetDescription, "SwapChain_Depth");
            var frameBuffer = this.dX11GraphicsContext.Factory.CreateFrameBuffer(new FrameBufferAttachment(rTDepthTarget, 0, 1), new[] { new FrameBufferAttachment(texture, 0, 1) });
            frameBuffer.SwapchainAssociated = true;
            display.FrameBuffer?.Dispose();
            display.UpdateFrameBuffer(frameBuffer);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var indexObj = (sender as FrameworkElement).Tag?.ToString();

            if (int.TryParse(indexObj, out var index))
            {
                this.interactionService.ColorIndex = index;
            }
        }

        private void OnDoorOpened(object sender, MouseButtonEventArgs e)
        {
            this.interactionService.DoorOpened = !this.interactionService.DoorOpened;
        }

        private void OnCameraChanged(object sender, MouseButtonEventArgs e)
        {
            this.interactionService.InsideCamera = !this.interactionService.InsideCamera;
        }
    }
}
