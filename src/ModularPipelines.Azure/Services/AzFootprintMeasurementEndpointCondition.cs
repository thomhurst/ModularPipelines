using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("footprint")]
public class AzFootprintMeasurementEndpointCondition
{
    public AzFootprintMeasurementEndpointCondition(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzFootprintMeasurementEndpointConditionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzFootprintMeasurementEndpointConditionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFootprintMeasurementEndpointConditionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzFootprintMeasurementEndpointConditionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzFootprintMeasurementEndpointConditionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFootprintMeasurementEndpointConditionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzFootprintMeasurementEndpointConditionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}