using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Helm.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Helm;

internal class Helm : IHelm
{
    private readonly ICommand _command;

    public Helm(ICommand command)
    {
        _command = command;
    }
    
    public Task<CommandResult> Install(HelmInstallOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("helm", "install"), cancellationToken);
    }

    public Task<CommandResult> Upgrade(HelmUpgradeOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("helm", "upgrade"), cancellationToken);
    }

    public Task<CommandResult> RegistryLogin(HelmRegistryLoginOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("helm", "registry", "login"), cancellationToken);
    }

    public Task<CommandResult> RegistryLogout(HelmOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("helm", "registry", "logout"), cancellationToken);
    }

    public Task<CommandResult> List(HelmListOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("helm", "list"), cancellationToken);
    }

    public Task<CommandResult> Rollback(HelmRollbackOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("helm", "rollback"), cancellationToken);
    }

    public Task<CommandResult> Uninstall(HelmUninstallOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options.ToCommandLineToolOptions("helm", "uninstall"), cancellationToken);
    }
}