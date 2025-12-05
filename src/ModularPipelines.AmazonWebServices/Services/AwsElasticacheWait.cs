using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache")]
public class AwsElasticacheWait
{
    public AwsElasticacheWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CacheClusterAvailable(AwsElasticacheWaitCacheClusterAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheWaitCacheClusterAvailableOptions(), token);
    }

    public async Task<CommandResult> CacheClusterDeleted(AwsElasticacheWaitCacheClusterDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheWaitCacheClusterDeletedOptions(), token);
    }

    public async Task<CommandResult> ReplicationGroupAvailable(AwsElasticacheWaitReplicationGroupAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheWaitReplicationGroupAvailableOptions(), token);
    }

    public async Task<CommandResult> ReplicationGroupDeleted(AwsElasticacheWaitReplicationGroupDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticacheWaitReplicationGroupDeletedOptions(), token);
    }
}