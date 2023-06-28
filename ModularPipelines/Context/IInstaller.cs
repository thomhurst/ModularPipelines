using CliWrap.Buffered;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface IInstaller
{
    Task<BufferedCommandResult> InstallFromFileAsync(InstallerOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> InstallFromWebAsync(WebInstallerOptions options,
        CancellationToken cancellationToken = default);
}