using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Examples.Modules;

public class NotepadPlusPlusInstallerModule : IModule<CommandResult>
{
    /// <inheritdoc/>
    public async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return await context.Installer.FileInstaller
            .InstallFromWebAsync(new WebInstallerOptions(new Uri(
                "https://github.com/notepad-plus-plus/notepad-plus-plus/releases/download/v8.5.3/npp.8.5.3.Installer.x64.exe")), cancellationToken);
    }
}