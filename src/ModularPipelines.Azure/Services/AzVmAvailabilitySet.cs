using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm")]
public class AzVmAvailabilitySet
{
    public AzVmAvailabilitySet(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Convert(AzVmAvailabilitySetConvertOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmAvailabilitySetConvertOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzVmAvailabilitySetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzVmAvailabilitySetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmAvailabilitySetDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzVmAvailabilitySetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmAvailabilitySetListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListSizes(AzVmAvailabilitySetListSizesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmAvailabilitySetListSizesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzVmAvailabilitySetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmAvailabilitySetShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzVmAvailabilitySetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmAvailabilitySetUpdateOptions(), cancellationToken: token);
    }
}