using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzNotificationHub
{
    public AzNotificationHub(
        AzNotificationHubAuthorizationRule authorizationRule,
        AzNotificationHubCredential credential,
        AzNotificationHubNamespace @namespace,
        ICommand internalCommand
    )
    {
        AuthorizationRule = authorizationRule;
        Credential = credential;
        Namespace = @namespace;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNotificationHubAuthorizationRule AuthorizationRule { get; }

    public AzNotificationHubCredential Credential { get; }

    public AzNotificationHubNamespace Namespace { get; }

    public async Task<CommandResult> CheckAvailability(AzNotificationHubCheckAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNotificationHubCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNotificationHubDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNotificationHubListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNotificationHubShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestSend(AzNotificationHubTestSendOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzNotificationHubUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubUpdateOptions(), token);
    }
}