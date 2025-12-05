using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container")]
public class GcloudContainerAws
{
    public GcloudContainerAws(
        GcloudContainerAwsClusters clusters,
        GcloudContainerAwsNodePools nodePools,
        GcloudContainerAwsOperations operations,
        ICommand internalCommand
    )
    {
        Clusters = clusters;
        NodePools = nodePools;
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerAwsClusters Clusters { get; }

    public GcloudContainerAwsNodePools NodePools { get; }

    public GcloudContainerAwsOperations Operations { get; }

    public async Task<CommandResult> GetServerConfig(GcloudContainerAwsGetServerConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerAwsGetServerConfigOptions(), token);
    }
}