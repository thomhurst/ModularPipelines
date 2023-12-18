using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid")]
public class AzEventgridSystemTopic
{
    public AzEventgridSystemTopic(
        AzEventgridSystemTopicCreate create,
        AzEventgridSystemTopicDelete delete,
        AzEventgridSystemTopicEventSubscription eventSubscription,
        AzEventgridSystemTopicList list,
        AzEventgridSystemTopicShow show,
        AzEventgridSystemTopicUpdate update,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        EventSubscription = eventSubscription;
        ListCommands = list;
        ShowCommands = show;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridSystemTopicCreate CreateCommands { get; }

    public AzEventgridSystemTopicDelete DeleteCommands { get; }

    public AzEventgridSystemTopicEventSubscription EventSubscription { get; }

    public AzEventgridSystemTopicList ListCommands { get; }

    public AzEventgridSystemTopicShow ShowCommands { get; }

    public AzEventgridSystemTopicUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzEventgridSystemTopicCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridSystemTopicDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridSystemTopicDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridSystemTopicListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridSystemTopicListOptions(), token);
    }

    public async Task<CommandResult> Show(AzEventgridSystemTopicShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridSystemTopicShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventgridSystemTopicUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridSystemTopicUpdateOptions(), token);
    }
}