using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace")]
public class AzEventgridPartnerNamespaceKey
{
    public AzEventgridPartnerNamespaceKey(
        AzEventgridPartnerNamespaceKeyList list,
        AzEventgridPartnerNamespaceKeyRegenerate regenerate,
        ICommand internalCommand
    )
    {
        ListCommands = list;
        RegenerateCommands = regenerate;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridPartnerNamespaceKeyList ListCommands { get; }

    public AzEventgridPartnerNamespaceKeyRegenerate RegenerateCommands { get; }

    public async Task<CommandResult> List(AzEventgridPartnerNamespaceKeyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Regenerate(AzEventgridPartnerNamespaceKeyRegenerateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}