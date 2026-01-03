using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventhubs", "eventhub")]
public class AzEventhubsEventhubConsumerGroup
{
    public AzEventhubsEventhubConsumerGroup(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzEventhubsEventhubConsumerGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzEventhubsEventhubConsumerGroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsEventhubConsumerGroupDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzEventhubsEventhubConsumerGroupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzEventhubsEventhubConsumerGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsEventhubConsumerGroupShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzEventhubsEventhubConsumerGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsEventhubConsumerGroupUpdateOptions(), cancellationToken: token);
    }
}