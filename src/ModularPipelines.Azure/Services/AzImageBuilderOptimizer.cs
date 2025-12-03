using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("image", "builder")]
public class AzImageBuilderOptimizer
{
    public AzImageBuilderOptimizer(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzImageBuilderOptimizerAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderOptimizerAddOptions(), token);
    }

    public async Task<CommandResult> Remove(AzImageBuilderOptimizerRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderOptimizerRemoveOptions(), token);
    }

    public async Task<CommandResult> Show(AzImageBuilderOptimizerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderOptimizerShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzImageBuilderOptimizerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderOptimizerUpdateOptions(), token);
    }
}