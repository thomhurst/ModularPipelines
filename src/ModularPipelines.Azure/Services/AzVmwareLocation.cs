using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware")]
public class AzVmwareLocation
{
    public AzVmwareLocation(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CheckQuotaAvailability(AzVmwareLocationCheckQuotaAvailabilityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareLocationCheckQuotaAvailabilityOptions(), token);
    }

    public async Task<CommandResult> CheckTrialAvailability(AzVmwareLocationCheckTrialAvailabilityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareLocationCheckTrialAvailabilityOptions(), token);
    }
}