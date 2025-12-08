using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("appstream")]
public class AwsAppstreamWait
{
    public AwsAppstreamWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> FleetStarted(AwsAppstreamWaitFleetStartedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamWaitFleetStartedOptions(), token);
    }

    public async Task<CommandResult> FleetStopped(AwsAppstreamWaitFleetStoppedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamWaitFleetStoppedOptions(), token);
    }
}