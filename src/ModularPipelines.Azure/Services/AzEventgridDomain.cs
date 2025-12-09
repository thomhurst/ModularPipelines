using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid")]
public class AzEventgridDomain
{
    public AzEventgridDomain(
        AzEventgridDomainCreate create,
        AzEventgridDomainDelete delete,
        AzEventgridDomainEventSubscription eventSubscription,
        AzEventgridDomainKey key,
        AzEventgridDomainList list,
        AzEventgridDomainPrivateEndpointConnection privateEndpointConnection,
        AzEventgridDomainPrivateLinkResource privateLinkResource,
        AzEventgridDomainShow show,
        AzEventgridDomainTopic topic,
        AzEventgridDomainUpdate update,
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
        Topic = topic;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridDomainCreate CreateCommands { get; }

    public AzEventgridDomainDelete DeleteCommands { get; }

    public AzEventgridDomainEventSubscription EventSubscription { get; }

    public AzEventgridDomainKey Key { get; }

    public AzEventgridDomainList ListCommands { get; }

    public AzEventgridDomainPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzEventgridDomainPrivateLinkResource PrivateLinkResource { get; }

    public AzEventgridDomainShow ShowCommands { get; }

    public AzEventgridDomainTopic Topic { get; }

    public AzEventgridDomainUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzEventgridDomainCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridDomainDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridDomainDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridDomainListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridDomainListOptions(), token);
    }

    public async Task<CommandResult> Show(AzEventgridDomainShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridDomainShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventgridDomainUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridDomainUpdateOptions(), token);
    }
}