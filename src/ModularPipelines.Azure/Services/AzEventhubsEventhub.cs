using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs")]
public class AzEventhubsEventhub
{
    public AzEventhubsEventhub(
        AzEventhubsEventhubAuthorizationRule authorizationRule,
        AzEventhubsEventhubConsumerGroup consumerGroup,
        ICommand internalCommand
    )
    {
        AuthorizationRule = authorizationRule;
        ConsumerGroup = consumerGroup;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventhubsEventhubAuthorizationRule AuthorizationRule { get; }

    public AzEventhubsEventhubConsumerGroup ConsumerGroup { get; }

    public async Task<CommandResult> Create(AzEventhubsEventhubCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventhubsEventhubDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsEventhubDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventhubsEventhubListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventhubsEventhubShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsEventhubShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventhubsEventhubUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsEventhubUpdateOptions(), token);
    }
}