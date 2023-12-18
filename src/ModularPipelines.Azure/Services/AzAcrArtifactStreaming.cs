using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr")]
public class AzAcrArtifactStreaming
{
    public AzAcrArtifactStreaming(
        AzAcrArtifactStreamingOperation operation,
        ICommand internalCommand
    )
    {
        Operation = operation;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAcrArtifactStreamingOperation Operation { get; }

    public async Task<CommandResult> Create(AzAcrArtifactStreamingCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAcrArtifactStreamingShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAcrArtifactStreamingUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}