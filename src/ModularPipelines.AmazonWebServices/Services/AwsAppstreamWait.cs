using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> FleetStarted(AwsAppstreamWaitFleetStartedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamWaitFleetStartedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> FleetStopped(AwsAppstreamWaitFleetStoppedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppstreamWaitFleetStoppedOptions(), executionOptions, cancellationToken);
    }
}