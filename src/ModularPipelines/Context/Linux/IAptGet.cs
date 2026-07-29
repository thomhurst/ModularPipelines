using ModularPipelines.Models;
using ModularPipelines.Options.Linux.AptGet;

namespace ModularPipelines.Context.Linux;

public interface IAptGet
{
    Task<CommandResult> AutocleanAsync(AptGetAutocleanOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> BuildDepAsync(AptGetBuildDepOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CheckAsync(AptGetCheckOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CleanAsync(AptGetCleanOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> DistUpgradeAsync(AptGetDistUpgradeOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> InstallAsync(AptGetInstallOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> PackageAsync(AptGetPackageOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> RemoveAsync(AptGetRemoveOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> SourceAsync(AptGetSourceOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> UpdateAsync(AptGetUpdateOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> UpgradeAsync(AptGetUpgradeOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CustomAsync(AptGetOptions options, CancellationToken cancellationToken = default);
}
