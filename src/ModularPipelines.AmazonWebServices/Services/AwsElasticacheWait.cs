using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("elasticache")]
public class AwsElasticacheWait
{
    public AwsElasticacheWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CacheClusterAvailable(AwsElasticacheWaitCacheClusterAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheWaitCacheClusterAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CacheClusterDeleted(AwsElasticacheWaitCacheClusterDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheWaitCacheClusterDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplicationGroupAvailable(AwsElasticacheWaitReplicationGroupAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheWaitReplicationGroupAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplicationGroupDeleted(AwsElasticacheWaitReplicationGroupDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheWaitReplicationGroupDeletedOptions(), executionOptions, cancellationToken);
    }
}