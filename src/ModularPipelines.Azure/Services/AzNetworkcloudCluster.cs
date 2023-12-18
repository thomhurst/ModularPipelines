using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud")]
public class AzNetworkcloudCluster
{
    public AzNetworkcloudCluster(
        AzNetworkcloudClusterBaremetalmachinekeyset baremetalmachinekeyset,
        AzNetworkcloudClusterBmckeyset bmckeyset,
        AzNetworkcloudClusterMetricsconfiguration metricsconfiguration,
        ICommand internalCommand
    )
    {
        Baremetalmachinekeyset = baremetalmachinekeyset;
        Bmckeyset = bmckeyset;
        Metricsconfiguration = metricsconfiguration;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkcloudClusterBaremetalmachinekeyset Baremetalmachinekeyset { get; }

    public AzNetworkcloudClusterBmckeyset Bmckeyset { get; }

    public AzNetworkcloudClusterMetricsconfiguration Metricsconfiguration { get; }

    public async Task<CommandResult> Create(AzNetworkcloudClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudClusterDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deploy(AzNetworkcloudClusterDeployOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkcloudClusterListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ScanRuntime(AzNetworkcloudClusterScanRuntimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudClusterShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudClusterUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVersion(AzNetworkcloudClusterUpdateVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudClusterWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudClusterWaitOptions(), token);
    }
}