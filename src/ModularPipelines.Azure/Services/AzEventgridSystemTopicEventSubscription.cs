using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "system-topic")]
public class AzEventgridSystemTopicEventSubscription
{
    public AzEventgridSystemTopicEventSubscription(
        AzEventgridSystemTopicEventSubscriptionCreate create,
        AzEventgridSystemTopicEventSubscriptionDelete delete,
        AzEventgridSystemTopicEventSubscriptionList list,
        AzEventgridSystemTopicEventSubscriptionShow show,
        AzEventgridSystemTopicEventSubscriptionUpdate update,
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

    public AzEventgridSystemTopicEventSubscriptionCreate CreateCommands { get; }

    public AzEventgridSystemTopicEventSubscriptionDelete DeleteCommands { get; }

    public AzEventgridSystemTopicEventSubscriptionList ListCommands { get; }

    public AzEventgridSystemTopicEventSubscriptionShow ShowCommands { get; }

    public AzEventgridSystemTopicEventSubscriptionUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzEventgridSystemTopicEventSubscriptionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridSystemTopicEventSubscriptionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzEventgridSystemTopicEventSubscriptionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventgridSystemTopicEventSubscriptionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzEventgridSystemTopicEventSubscriptionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}