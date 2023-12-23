using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk")]
public class AwsElasticbeanstalkWait
{
    public AwsElasticbeanstalkWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> EnvironmentExists(AwsElasticbeanstalkWaitEnvironmentExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkWaitEnvironmentExistsOptions(), token);
    }

    public async Task<CommandResult> EnvironmentTerminated(AwsElasticbeanstalkWaitEnvironmentTerminatedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkWaitEnvironmentTerminatedOptions(), token);
    }

    public async Task<CommandResult> EnvironmentUpdated(AwsElasticbeanstalkWaitEnvironmentUpdatedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkWaitEnvironmentUpdatedOptions(), token);
    }
}