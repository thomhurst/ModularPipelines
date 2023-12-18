using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "domain")]
public class AzEventgridDomainTopic
{
    public AzEventgridDomainTopic(
        AzEventgridDomainTopicCreate create,
        AzEventgridDomainTopicDelete delete,
        AzEventgridDomainTopicEventSubscription eventSubscription,
        AzEventgridDomainTopicList list,
        AzEventgridDomainTopicShow show,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        EventSubscription = eventSubscription;
        ListCommands = list;
        ShowCommands = show;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridDomainTopicCreate CreateCommands { get; }

    public AzEventgridDomainTopicDelete DeleteCommands { get; }

    public AzEventgridDomainTopicEventSubscription EventSubscription { get; }

    public AzEventgridDomainTopicList ListCommands { get; }

    public AzEventgridDomainTopicShow ShowCommands { get; }

    public async Task<CommandResult> Create(AzEventgridDomainTopicCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridDomainTopicDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridDomainTopicDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridDomainTopicListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventgridDomainTopicShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridDomainTopicShowOptions(), token);
    }
}