using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner")]
public class AzEventgridPartnerTopic
{
    public AzEventgridPartnerTopic(
        AzEventgridPartnerTopicActivate activate,
        AzEventgridPartnerTopicDeactivate deactivate,
        AzEventgridPartnerTopicDelete delete,
        AzEventgridPartnerTopicEventSubscription eventSubscription,
        AzEventgridPartnerTopicList list,
        AzEventgridPartnerTopicShow show,
        ICommand internalCommand
    )
    {
        ActivateCommands = activate;
        DeactivateCommands = deactivate;
        DeleteCommands = delete;
        EventSubscription = eventSubscription;
        ListCommands = list;
        ShowCommands = show;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridPartnerTopicActivate ActivateCommands { get; }

    public AzEventgridPartnerTopicDeactivate DeactivateCommands { get; }

    public AzEventgridPartnerTopicDelete DeleteCommands { get; }

    public AzEventgridPartnerTopicEventSubscription EventSubscription { get; }

    public AzEventgridPartnerTopicList ListCommands { get; }

    public AzEventgridPartnerTopicShow ShowCommands { get; }

    public async Task<CommandResult> Activate(AzEventgridPartnerTopicActivateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerTopicActivateOptions(), token);
    }

    public async Task<CommandResult> Deactivate(AzEventgridPartnerTopicDeactivateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerTopicDeactivateOptions(), token);
    }

    public async Task<CommandResult> Delete(AzEventgridPartnerTopicDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerTopicDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridPartnerTopicListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerTopicListOptions(), token);
    }

    public async Task<CommandResult> Show(AzEventgridPartnerTopicShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerTopicShowOptions(), token);
    }
}

