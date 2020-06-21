using Flare_Sharp.ClientBase.Categories;
using Flare_Sharp.Memory.FlameSDK;

namespace Flare_Sharp.ClientBase.Modules.Modules
{
    public class HiveFly : Module
    {
        int delayCount = 0;
        public HiveFly() : base("Hive Fly", CategoryHandler.registry.categories[1], (char)0x07, false)
        {
        }

        public override void onDisable()
        {
            base.onDisable();
        }

        public override void onTick()
        {
            base.onTick();
            Minecraft.clientInstance.localPlayer.velY = 0;
            if (delayCount >= 45)
            {
                Minecraft.clientInstance.localPlayer.teleport(Minecraft.clientInstance.localPlayer.X1, Minecraft.clientInstance.localPlayer.Y1 - 0.001f, Minecraft.clientInstance.localPlayer.Z1);
                delayCount = 0;
            }
            delayCount++;
        }
    }
}
