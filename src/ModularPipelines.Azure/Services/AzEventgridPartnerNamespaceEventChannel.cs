using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace")]
public class AzEventgridPartnerNamespaceEventChannel
{
    public AzEventgridPartnerNamespaceEventChannel(
        AzEventgridPartnerNamespaceEventChannelCreate create,
        AzEventgridPartnerNamespaceEventChannelDelete delete,
        AzEventgridPartnerNamespaceEventChannelList list,
        AzEventgridPartnerNamespaceEventChannelShow show,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        ListCommands = list;
        ShowCommands = show;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridPartnerNamespaceEventChannelCreate CreateCommands { get; }

    public AzEventgridPartnerNamespaceEventChannelDelete DeleteCommands { get; }

    public AzEventgridPartnerNamespaceEventChannelList ListCommands { get; }

    public AzEventgridPartnerNamespaceEventChannelShow ShowCommands { get; }

    public async Task<CommandResult> Create(AzEventgridPartnerNamespaceEventChannelCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridPartnerNamespaceEventChannelDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzEventgridPartnerNamespaceEventChannelListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventgridPartnerNamespaceEventChannelShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerNamespaceEventChannelShowOptions(), token);
    }
}

