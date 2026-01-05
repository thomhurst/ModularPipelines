using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("elasticbeanstalk")]
public class AwsElasticbeanstalkWait
{
    public AwsElasticbeanstalkWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> EnvironmentExists(AwsElasticbeanstalkWaitEnvironmentExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkWaitEnvironmentExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnvironmentTerminated(AwsElasticbeanstalkWaitEnvironmentTerminatedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkWaitEnvironmentTerminatedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnvironmentUpdated(AwsElasticbeanstalkWaitEnvironmentUpdatedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkWaitEnvironmentUpdatedOptions(), executionOptions, cancellationToken);
    }
}