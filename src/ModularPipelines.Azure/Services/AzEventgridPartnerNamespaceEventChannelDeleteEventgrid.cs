using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "event-channel", "delete")]
public class AzEventgridPartnerNamespaceEventChannelDeleteEventgrid
{
    public AzEventgridPartnerNamespaceEventChannelDeleteEventgrid(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzEventgridPartnerNamespaceEventChannelDeleteEventgridExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridPartnerNamespaceEventChannelDeleteEventgridExtensionOptions(), token);
    }
}