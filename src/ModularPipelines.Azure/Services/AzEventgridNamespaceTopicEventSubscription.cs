using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "namespace", "topic")]
public class AzEventgridNamespaceTopicEventSubscription
{
    public AzEventgridNamespaceTopicEventSubscription(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzEventgridNamespaceTopicEventSubscriptionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridNamespaceTopicEventSubscriptionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceTopicEventSubscriptionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridNamespaceTopicEventSubscriptionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventgridNamespaceTopicEventSubscriptionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceTopicEventSubscriptionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventgridNamespaceTopicEventSubscriptionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceTopicEventSubscriptionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzEventgridNamespaceTopicEventSubscriptionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceTopicEventSubscriptionWaitOptions(), token);
    }
}