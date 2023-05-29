using ModularPipelines.Context;
using ModularPipelines.Installer;

namespace ModularPipelines.Examples.Modules;

public class NotepadPlusPlusInstallerModule : WebInstallerModule
{
    public NotepadPlusPlusInstallerModule(IModuleContext context) : base(context)
    {
    }

    protected override Uri DownloadUri { get; } =
        new("https://github.com/notepad-plus-plus/notepad-plus-plus/releases/download/v8.5.3/npp.8.5.3.Installer.x64.exe");
}