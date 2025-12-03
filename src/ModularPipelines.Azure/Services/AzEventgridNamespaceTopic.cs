using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "namespace")]
public class AzEventgridNamespaceTopic
{
    public AzEventgridNamespaceTopic(
        AzEventgridNamespaceTopicEventSubscription eventSubscription,
        ICommand internalCommand
    )
    {
        EventSubscription = eventSubscription;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridNamespaceTopicEventSubscription EventSubscription { get; }

    public async Task<CommandResult> Create(AzEventgridNamespaceTopicCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridNamespaceTopicDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceTopicDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridNamespaceTopicListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListKey(AzEventgridNamespaceTopicListKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceTopicListKeyOptions(), token);
    }

    public async Task<CommandResult> RegenerateKey(AzEventgridNamespaceTopicRegenerateKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventgridNamespaceTopicShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceTopicShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventgridNamespaceTopicUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceTopicUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzEventgridNamespaceTopicWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceTopicWaitOptions(), token);
    }
}