using ModularPipelines.Models;
using ModularPipelines.Options.Linux.AptGet;

namespace ModularPipelines.Context.Linux;

public interface IAptGet
{
    Task<CommandResult> Autoclean(AptGetAutocleanOptions? options = default, CancellationToken token = default);

    Task<CommandResult> BuildDep(AptGetBuildDepOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Check(AptGetCheckOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Clean(AptGetCleanOptions? options = default, CancellationToken token = default);

    Task<CommandResult> DistUpgrade(AptGetDistUpgradeOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Install(AptGetInstallOptions options, CancellationToken token = default);

    Task<CommandResult> Package(AptGetPackageOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Remove(AptGetRemoveOptions options, CancellationToken token = default);

    Task<CommandResult> Source(AptGetSourceOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Update(AptGetUpdateOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Upgrade(AptGetUpgradeOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Custom(AptGetOptions options, CancellationToken token = default);
}