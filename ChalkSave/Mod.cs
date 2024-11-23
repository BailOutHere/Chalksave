using GDWeave;
using ChalkSave.Mods;
namespace ChalkImageDraw;


public class Mod : IMod {

    public Mod(IModInterface modInterface) {
        modInterface.Logger.Information("ChalkSave loaded!");
        modInterface.RegisterScriptMod(new InjectChalkCanvas());
        modInterface.RegisterScriptMod(new InjectPlayerData());
        modInterface.RegisterScriptMod(new InjectPlayerHud());
        modInterface.RegisterScriptMod(new InjectPlayer());
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
