using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "topic")]
public class AzEventgridPartnerTopicEventSubscription
{
    public AzEventgridPartnerTopicEventSubscription(
        AzEventgridPartnerTopicEventSubscriptionCreate create,
        AzEventgridPartnerTopicEventSubscriptionDelete delete,
        AzEventgridPartnerTopicEventSubscriptionList list,
        AzEventgridPartnerTopicEventSubscriptionShow show,
        AzEventgridPartnerTopicEventSubscriptionUpdate update,
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

    public AzEventgridPartnerTopicEventSubscriptionCreate CreateCommands { get; }

    public AzEventgridPartnerTopicEventSubscriptionDelete DeleteCommands { get; }

    public AzEventgridPartnerTopicEventSubscriptionList ListCommands { get; }

    public AzEventgridPartnerTopicEventSubscriptionShow ShowCommands { get; }

    public AzEventgridPartnerTopicEventSubscriptionUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzEventgridPartnerTopicEventSubscriptionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridPartnerTopicEventSubscriptionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzEventgridPartnerTopicEventSubscriptionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventgridPartnerTopicEventSubscriptionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzEventgridPartnerTopicEventSubscriptionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}