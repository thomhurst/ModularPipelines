using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid")]
public class AzEventgridEventSubscription
{
    public AzEventgridEventSubscription(
        AzEventgridEventSubscriptionCreate create,
        AzEventgridEventSubscriptionDelete delete,
        AzEventgridEventSubscriptionList list,
        AzEventgridEventSubscriptionShow show,
        AzEventgridEventSubscriptionUpdate update,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        ListCommands = list;
        ShowCommands = show;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridEventSubscriptionCreate CreateCommands { get; }

    public AzEventgridEventSubscriptionDelete DeleteCommands { get; }

    public AzEventgridEventSubscriptionList ListCommands { get; }

    public AzEventgridEventSubscriptionShow ShowCommands { get; }

    public AzEventgridEventSubscriptionUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzEventgridEventSubscriptionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridEventSubscriptionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzEventgridEventSubscriptionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventgridEventSubscriptionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzEventgridEventSubscriptionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

