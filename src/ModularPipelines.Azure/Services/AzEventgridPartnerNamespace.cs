using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner")]
public class AzEventgridPartnerNamespace
{
    public AzEventgridPartnerNamespace(
        AzEventgridPartnerNamespaceChannel channel,
        AzEventgridPartnerNamespaceCreate create,
        AzEventgridPartnerNamespaceDelete delete,
        AzEventgridPartnerNamespaceEventChannel eventChannel,
        AzEventgridPartnerNamespaceKey key,
        AzEventgridPartnerNamespaceList list,
        AzEventgridPartnerNamespaceShow show,
        ICommand internalCommand
    )
    {
        Channel = channel;
        CreateCommands = create;
        DeleteCommands = delete;
        EventChannel = eventChannel;
        Key = key;
        ListCommands = list;
        ShowCommands = show;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridPartnerNamespaceChannel Channel { get; }

    public AzEventgridPartnerNamespaceCreate CreateCommands { get; }

    public AzEventgridPartnerNamespaceDelete DeleteCommands { get; }

    public AzEventgridPartnerNamespaceEventChannel EventChannel { get; }

    public AzEventgridPartnerNamespaceKey Key { get; }

    public AzEventgridPartnerNamespaceList ListCommands { get; }

    public AzEventgridPartnerNamespaceShow ShowCommands { get; }

    public async Task<CommandResult> Create(AzEventgridPartnerNamespaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridPartnerNamespaceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerNamespaceDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridPartnerNamespaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerNamespaceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzEventgridPartnerNamespaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerNamespaceShowOptions(), token);
    }
}