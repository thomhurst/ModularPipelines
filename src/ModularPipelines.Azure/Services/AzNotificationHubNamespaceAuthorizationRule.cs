using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("notification-hub", "namespace")]
public class AzNotificationHubNamespaceAuthorizationRule
{
    public AzNotificationHubNamespaceAuthorizationRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNotificationHubNamespaceAuthorizationRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNotificationHubNamespaceAuthorizationRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceAuthorizationRuleDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNotificationHubNamespaceAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListKeys(AzNotificationHubNamespaceAuthorizationRuleListKeysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceAuthorizationRuleListKeysOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> RegenerateKeys(AzNotificationHubNamespaceAuthorizationRuleRegenerateKeysOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNotificationHubNamespaceAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubNamespaceAuthorizationRuleShowOptions(), cancellationToken: token);
    }
}