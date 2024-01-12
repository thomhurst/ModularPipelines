using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds")]
public class GcloudVmwarePrivateCloudsClusters
{
    public GcloudVmwarePrivateCloudsClusters(
        GcloudVmwarePrivateCloudsClustersNodes nodes,
        ICommand internalCommand
    )
    {
        Nodes = nodes;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudVmwarePrivateCloudsClustersNodes Nodes { get; }

    public async Task<CommandResult> Create(GcloudVmwarePrivateCloudsClustersCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudVmwarePrivateCloudsClustersDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudVmwarePrivateCloudsClustersDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudVmwarePrivateCloudsClustersListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudVmwarePrivateCloudsClustersUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}