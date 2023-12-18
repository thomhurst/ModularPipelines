using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid")]
public class AzEventgridTopic
{
    public AzEventgridTopic(
        AzEventgridTopicCreate create,
        AzEventgridTopicDelete delete,
        AzEventgridTopicEventSubscription eventSubscription,
        AzEventgridTopicKey key,
        AzEventgridTopicList list,
        AzEventgridTopicPrivateEndpointConnection privateEndpointConnection,
        AzEventgridTopicPrivateLinkResource privateLinkResource,
        AzEventgridTopicShow show,
        AzEventgridTopicUpdate update,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        EventSubscription = eventSubscription;
        Key = key;
        ListCommands = list;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        ShowCommands = show;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridTopicCreate CreateCommands { get; }

    public AzEventgridTopicDelete DeleteCommands { get; }

    public AzEventgridTopicEventSubscription EventSubscription { get; }

    public AzEventgridTopicKey Key { get; }

    public AzEventgridTopicList ListCommands { get; }

    public AzEventgridTopicPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzEventgridTopicPrivateLinkResource PrivateLinkResource { get; }

    public AzEventgridTopicShow ShowCommands { get; }

    public AzEventgridTopicUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzEventgridTopicCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridTopicDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridTopicDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridTopicListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridTopicListOptions(), token);
    }

    public async Task<CommandResult> Show(AzEventgridTopicShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridTopicShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventgridTopicUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridTopicUpdateOptions(), token);
    }
}