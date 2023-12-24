using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsDrs
{
    public AwsDrs(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateSourceNetworkStack(AwsDrsAssociateSourceNetworkStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateExtendedSourceServer(AwsDrsCreateExtendedSourceServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLaunchConfigurationTemplate(AwsDrsCreateLaunchConfigurationTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDrsCreateLaunchConfigurationTemplateOptions(), token);
    }

    public async Task<CommandResult> CreateReplicationConfigurationTemplate(AwsDrsCreateReplicationConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSourceNetwork(AwsDrsCreateSourceNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteJob(AwsDrsDeleteJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLaunchAction(AwsDrsDeleteLaunchActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLaunchConfigurationTemplate(AwsDrsDeleteLaunchConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRecoveryInstance(AwsDrsDeleteRecoveryInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReplicationConfigurationTemplate(AwsDrsDeleteReplicationConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSourceNetwork(AwsDrsDeleteSourceNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSourceServer(AwsDrsDeleteSourceServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeJobLogItems(AwsDrsDescribeJobLogItemsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeJobs(AwsDrsDescribeJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDrsDescribeJobsOptions(), token);
    }

    public async Task<CommandResult> DescribeLaunchConfigurationTemplates(AwsDrsDescribeLaunchConfigurationTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDrsDescribeLaunchConfigurationTemplatesOptions(), token);
    }

    public async Task<CommandResult> DescribeRecoveryInstances(AwsDrsDescribeRecoveryInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDrsDescribeRecoveryInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeRecoverySnapshots(AwsDrsDescribeRecoverySnapshotsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeReplicationConfigurationTemplates(AwsDrsDescribeReplicationConfigurationTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDrsDescribeReplicationConfigurationTemplatesOptions(), token);
    }

    public async Task<CommandResult> DescribeSourceNetworks(AwsDrsDescribeSourceNetworksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDrsDescribeSourceNetworksOptions(), token);
    }

    public async Task<CommandResult> DescribeSourceServers(AwsDrsDescribeSourceServersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDrsDescribeSourceServersOptions(), token);
    }

    public async Task<CommandResult> DisconnectRecoveryInstance(AwsDrsDisconnectRecoveryInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisconnectSourceServer(AwsDrsDisconnectSourceServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportSourceNetworkCfnTemplate(AwsDrsExportSourceNetworkCfnTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFailbackReplicationConfiguration(AwsDrsGetFailbackReplicationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLaunchConfiguration(AwsDrsGetLaunchConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReplicationConfiguration(AwsDrsGetReplicationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InitializeService(AwsDrsInitializeServiceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDrsInitializeServiceOptions(), token);
    }

    public async Task<CommandResult> ListExtensibleSourceServers(AwsDrsListExtensibleSourceServersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLaunchActions(AwsDrsListLaunchActionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStagingAccounts(AwsDrsListStagingAccountsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDrsListStagingAccountsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsDrsListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutLaunchAction(AwsDrsPutLaunchActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReverseReplication(AwsDrsReverseReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartFailbackLaunch(AwsDrsStartFailbackLaunchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartRecovery(AwsDrsStartRecoveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartReplication(AwsDrsStartReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSourceNetworkRecovery(AwsDrsStartSourceNetworkRecoveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSourceNetworkReplication(AwsDrsStartSourceNetworkReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopFailback(AwsDrsStopFailbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopReplication(AwsDrsStopReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopSourceNetworkReplication(AwsDrsStopSourceNetworkReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsDrsTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TerminateRecoveryInstances(AwsDrsTerminateRecoveryInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsDrsUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFailbackReplicationConfiguration(AwsDrsUpdateFailbackReplicationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLaunchConfiguration(AwsDrsUpdateLaunchConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLaunchConfigurationTemplate(AwsDrsUpdateLaunchConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateReplicationConfiguration(AwsDrsUpdateReplicationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateReplicationConfigurationTemplate(AwsDrsUpdateReplicationConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}