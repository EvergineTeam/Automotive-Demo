using WaveEngine.Common.Graphics;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Mathematics;

namespace AutomotiveDemo
{
    public class MyScene : Scene
    {
		public override void RegisterManagers()
        {
        	base.RegisterManagers();
        	this.Managers.AddManager(new WaveEngine.Bullet.BulletPhysicManager3D());        	
        }

        protected override void CreateScene()
        {
        }
    }
}