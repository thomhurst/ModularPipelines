using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "domain")]
public class AzEventgridDomainKey
{
    public AzEventgridDomainKey(
        AzEventgridDomainKeyList list,
        AzEventgridDomainKeyRegenerate regenerate,
        ICommand internalCommand
    )
    {
        ListCommands = list;
        RegenerateCommands = regenerate;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridDomainKeyList ListCommands { get; }

    public AzEventgridDomainKeyRegenerate RegenerateCommands { get; }

    public async Task<CommandResult> List(AzEventgridDomainKeyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Regenerate(AzEventgridDomainKeyRegenerateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}