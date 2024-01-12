using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container")]
public class GcloudEdgeCloudContainerClusters
{
    public GcloudEdgeCloudContainerClusters(
        GcloudEdgeCloudContainerClustersNodePools nodePools,
        ICommand internalCommand
    )
    {
        NodePools = nodePools;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudEdgeCloudContainerClustersNodePools NodePools { get; }

    public async Task<CommandResult> Create(GcloudEdgeCloudContainerClustersCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudEdgeCloudContainerClustersDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudEdgeCloudContainerClustersDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCredentials(GcloudEdgeCloudContainerClustersGetCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudEdgeCloudContainerClustersListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudEdgeCloudContainerClustersListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudEdgeCloudContainerClustersUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Upgrade(GcloudEdgeCloudContainerClustersUpgradeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}