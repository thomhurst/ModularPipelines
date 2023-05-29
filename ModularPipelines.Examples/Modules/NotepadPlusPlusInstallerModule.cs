using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Installer.Extensions;
using ModularPipelines.Installer.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class NotepadPlusPlusInstallerModule : Module<BufferedCommandResult>
{
    public NotepadPlusPlusInstallerModule(IModuleContext context) : base(context)
    {
    }

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await Context.Installer()
            .InstallFromWeb(new WebInstallerOptions(new Uri(
                "https://github.com/notepad-plus-plus/notepad-plus-plus/releases/download/v8.5.3/npp.8.5.3.Installer.x64.exe")), cancellationToken);
    }
}