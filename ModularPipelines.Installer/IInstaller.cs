using CliWrap.Buffered;
using ModularPipelines.Installer.Options;

namespace ModularPipelines.Installer;

public interface IInstaller
{
    Task<BufferedCommandResult> InstallFromFile(InstallerOptions options, CancellationToken cancellationToken = default);
    Task<BufferedCommandResult> InstallFromWeb(WebInstallerOptions options,
        CancellationToken cancellationToken = default);
}

public interface IInstaller<T> : IInstaller
{
}