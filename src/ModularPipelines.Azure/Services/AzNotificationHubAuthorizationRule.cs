using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub")]
public class AzNotificationHubAuthorizationRule
{
    public AzNotificationHubAuthorizationRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNotificationHubAuthorizationRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNotificationHubAuthorizationRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubAuthorizationRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNotificationHubAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListKeys(AzNotificationHubAuthorizationRuleListKeysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubAuthorizationRuleListKeysOptions(), token);
    }

    public async Task<CommandResult> RegenerateKeys(AzNotificationHubAuthorizationRuleRegenerateKeysOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNotificationHubAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNotificationHubAuthorizationRuleShowOptions(), token);
    }
}