using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNotificationHubNamespaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNotificationHubNamespaceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNotificationHubNamespaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNotificationHubNamespaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNotificationHubNamespaceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNotificationHubNamespaceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceWaitOptions(), token);
    }
}