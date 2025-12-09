using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks")]
public class AzAksApprouting
{
    public AzAksApprouting(
        AzAksApproutingZone zone,
        ICommand internalCommand
    )
    {
        Zone = zone;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAksApproutingZone Zone { get; }

    public async Task<CommandResult> Disable(AzAksApproutingDisableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Enable(AzAksApproutingEnableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAksApproutingUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}