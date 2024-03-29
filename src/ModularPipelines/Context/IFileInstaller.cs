using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface IFileInstaller
{
    Task<CommandResult> InstallFromFileAsync(InstallerOptions options,
        CancellationToken cancellationToken = default);

    Task<CommandResult> InstallFromWebAsync(WebInstallerOptions options,
        CancellationToken cancellationToken = default);
}