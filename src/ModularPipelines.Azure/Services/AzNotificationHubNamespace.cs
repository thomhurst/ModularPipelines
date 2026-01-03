using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("notification-hub")]
public class AzNotificationHubNamespace
{
    public AzNotificationHubNamespace(
        AzNotificationHubNamespaceAuthorizationRule authorizationRule,
        ICommand internalCommand
    )
    {
        AuthorizationRule = authorizationRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNotificationHubNamespaceAuthorizationRule AuthorizationRule { get; }

    public async Task<CommandResult> CheckAvailability(AzNotificationHubNamespaceCheckAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzNotificationHubNamespaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNotificationHubNamespaceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNotificationHubNamespaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNotificationHubNamespaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNotificationHubNamespaceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNotificationHubNamespaceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceWaitOptions(), cancellationToken: token);
    }
}