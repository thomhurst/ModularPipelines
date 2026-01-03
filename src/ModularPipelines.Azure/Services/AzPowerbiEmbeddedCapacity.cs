using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("powerbi")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzPowerbiEmbeddedCapacityDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPowerbiEmbeddedCapacityDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzPowerbiEmbeddedCapacityListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPowerbiEmbeddedCapacityListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzPowerbiEmbeddedCapacityShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPowerbiEmbeddedCapacityShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzPowerbiEmbeddedCapacityUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPowerbiEmbeddedCapacityUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzPowerbiEmbeddedCapacityWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPowerbiEmbeddedCapacityWaitOptions(), cancellationToken: token);
    }
}