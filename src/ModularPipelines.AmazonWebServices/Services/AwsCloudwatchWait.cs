using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> AlarmExists(AwsCloudwatchWaitAlarmExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchWaitAlarmExistsOptions(), token);
    }

    public async Task<CommandResult> CompositeAlarmExists(AwsCloudwatchWaitCompositeAlarmExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudwatchWaitCompositeAlarmExistsOptions(), token);
    }
}