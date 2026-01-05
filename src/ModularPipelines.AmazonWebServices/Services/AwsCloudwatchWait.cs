using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloudwatch")]
public class AwsCloudwatchWait
{
    public AwsCloudwatchWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AlarmExists(AwsCloudwatchWaitAlarmExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchWaitAlarmExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CompositeAlarmExists(AwsCloudwatchWaitCompositeAlarmExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchWaitCompositeAlarmExistsOptions(), executionOptions, cancellationToken);
    }
}