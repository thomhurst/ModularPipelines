using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp")]
public class AzFunctionappPlan
{
    public AzFunctionappPlan(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzFunctionappPlanCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzFunctionappPlanDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappPlanDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzFunctionappPlanListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappPlanListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzFunctionappPlanShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappPlanShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzFunctionappPlanUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappPlanUpdateOptions(), cancellationToken: token);
    }
}