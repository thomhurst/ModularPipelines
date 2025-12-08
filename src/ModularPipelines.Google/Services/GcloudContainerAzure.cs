using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("container")]
public class GcloudContainerAzure
{
    public GcloudContainerAzure(
        GcloudContainerAzureClients clients,
        GcloudContainerAzureClusters clusters,
        GcloudContainerAzureNodePools nodePools,
        GcloudContainerAzureOperations operations,
        ICommand internalCommand
    )
    {
        Clients = clients;
        Clusters = clusters;
        NodePools = nodePools;
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerAzureClients Clients { get; }

    public GcloudContainerAzureClusters Clusters { get; }

    public GcloudContainerAzureNodePools NodePools { get; }

    public GcloudContainerAzureOperations Operations { get; }

    public async Task<CommandResult> GetServerConfig(GcloudContainerAzureGetServerConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerAzureGetServerConfigOptions(), token);
    }
}