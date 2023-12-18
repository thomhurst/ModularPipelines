using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("powerbi")]
public class AzPowerbiEmbeddedCapacity
{
    public AzPowerbiEmbeddedCapacity(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzPowerbiEmbeddedCapacityCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPowerbiEmbeddedCapacityDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPowerbiEmbeddedCapacityDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzPowerbiEmbeddedCapacityListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPowerbiEmbeddedCapacityListOptions(), token);
    }

    public async Task<CommandResult> Show(AzPowerbiEmbeddedCapacityShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPowerbiEmbeddedCapacityShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzPowerbiEmbeddedCapacityUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPowerbiEmbeddedCapacityUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzPowerbiEmbeddedCapacityWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPowerbiEmbeddedCapacityWaitOptions(), token);
    }
}

