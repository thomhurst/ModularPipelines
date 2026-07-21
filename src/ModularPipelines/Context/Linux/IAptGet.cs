using ModularPipelines.Models;
using ModularPipelines.Options.Linux.AptGet;

namespace ModularPipelines.Context.Linux;

public interface IAptGet
{
    Task<CommandResult> Autoclean(AptGetAutocleanOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> BuildDep(AptGetBuildDepOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Check(AptGetCheckOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Clean(AptGetCleanOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> DistUpgrade(AptGetDistUpgradeOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Install(AptGetInstallOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Package(AptGetPackageOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Remove(AptGetRemoveOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Source(AptGetSourceOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Update(AptGetUpdateOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Upgrade(AptGetUpgradeOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Custom(AptGetOptions options, CancellationToken cancellationToken = default);
}