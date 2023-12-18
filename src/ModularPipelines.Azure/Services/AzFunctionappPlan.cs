using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzFunctionappPlanDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappPlanDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzFunctionappPlanListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappPlanListOptions(), token);
    }

    public async Task<CommandResult> Show(AzFunctionappPlanShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappPlanShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzFunctionappPlanUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappPlanUpdateOptions(), token);
    }
}