using ModularPipelines.Helm.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Helm;

public interface IHelm
{
    Task<CommandResult> Install(HelmInstallOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Upgrade(HelmUpgradeOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> RegistryLogin(HelmRegistryLoginOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> RegistryLogout(HelmOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> List(HelmListOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Rollback(HelmRollbackOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Uninstall(HelmUninstallOptions options, CancellationToken cancellationToken = default);
}
