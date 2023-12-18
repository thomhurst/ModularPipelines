using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("image", "builder")]
public class AzImageBuilderOutput
{
    public AzImageBuilderOutput(
        AzImageBuilderOutputVersioning versioning,
        ICommand internalCommand
    )
    {
        Versioning = versioning;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzImageBuilderOutputVersioning Versioning { get; }

    public async Task<CommandResult> Add(AzImageBuilderOutputAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Clear(AzImageBuilderOutputClearOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzImageBuilderOutputRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

