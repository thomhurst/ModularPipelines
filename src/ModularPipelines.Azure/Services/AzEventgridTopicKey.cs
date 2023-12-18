using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic")]
public class AzEventgridTopicKey
{
    public AzEventgridTopicKey(
        AzEventgridTopicKeyList list,
        AzEventgridTopicKeyRegenerate regenerate,
        ICommand internalCommand
    )
    {
        ListCommands = list;
        RegenerateCommands = regenerate;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventgridTopicKeyList ListCommands { get; }

    public AzEventgridTopicKeyRegenerate RegenerateCommands { get; }

    public async Task<CommandResult> List(AzEventgridTopicKeyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Regenerate(AzEventgridTopicKeyRegenerateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

