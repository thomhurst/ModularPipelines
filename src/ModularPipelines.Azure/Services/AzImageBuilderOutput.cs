using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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

    public async Task<CommandResult> Add(AzImageBuilderOutputAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderOutputAddOptions(), token);
    }

    public async Task<CommandResult> Clear(AzImageBuilderOutputClearOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderOutputClearOptions(), token);
    }

    public async Task<CommandResult> Remove(AzImageBuilderOutputRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}