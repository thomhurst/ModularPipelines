using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsMgn
{
    public AwsMgn(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ArchiveApplication(AwsMgnArchiveApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ArchiveWave(AwsMgnArchiveWaveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateApplications(AwsMgnAssociateApplicationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateSourceServers(AwsMgnAssociateSourceServersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ChangeServerLifeCycleState(AwsMgnChangeServerLifeCycleStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateApplication(AwsMgnCreateApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConnector(AwsMgnCreateConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLaunchConfigurationTemplate(AwsMgnCreateLaunchConfigurationTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnCreateLaunchConfigurationTemplateOptions(), token);
    }

    public async Task<CommandResult> CreateReplicationConfigurationTemplate(AwsMgnCreateReplicationConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWave(AwsMgnCreateWaveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApplication(AwsMgnDeleteApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConnector(AwsMgnDeleteConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteJob(AwsMgnDeleteJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLaunchConfigurationTemplate(AwsMgnDeleteLaunchConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReplicationConfigurationTemplate(AwsMgnDeleteReplicationConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSourceServer(AwsMgnDeleteSourceServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVcenterClient(AwsMgnDeleteVcenterClientOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWave(AwsMgnDeleteWaveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeJobLogItems(AwsMgnDescribeJobLogItemsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeJobs(AwsMgnDescribeJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnDescribeJobsOptions(), token);
    }

    public async Task<CommandResult> DescribeLaunchConfigurationTemplates(AwsMgnDescribeLaunchConfigurationTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnDescribeLaunchConfigurationTemplatesOptions(), token);
    }

    public async Task<CommandResult> DescribeReplicationConfigurationTemplates(AwsMgnDescribeReplicationConfigurationTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnDescribeReplicationConfigurationTemplatesOptions(), token);
    }

    public async Task<CommandResult> DescribeSourceServers(AwsMgnDescribeSourceServersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnDescribeSourceServersOptions(), token);
    }

    public async Task<CommandResult> DescribeVcenterClients(AwsMgnDescribeVcenterClientsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnDescribeVcenterClientsOptions(), token);
    }

    public async Task<CommandResult> DisassociateApplications(AwsMgnDisassociateApplicationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateSourceServers(AwsMgnDisassociateSourceServersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisconnectFromService(AwsMgnDisconnectFromServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FinalizeCutover(AwsMgnFinalizeCutoverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLaunchConfiguration(AwsMgnGetLaunchConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReplicationConfiguration(AwsMgnGetReplicationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InitializeService(AwsMgnInitializeServiceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnInitializeServiceOptions(), token);
    }

    public async Task<CommandResult> ListApplications(AwsMgnListApplicationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnListApplicationsOptions(), token);
    }

    public async Task<CommandResult> ListConnectors(AwsMgnListConnectorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnListConnectorsOptions(), token);
    }

    public async Task<CommandResult> ListExportErrors(AwsMgnListExportErrorsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListExports(AwsMgnListExportsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnListExportsOptions(), token);
    }

    public async Task<CommandResult> ListImportErrors(AwsMgnListImportErrorsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListImports(AwsMgnListImportsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnListImportsOptions(), token);
    }

    public async Task<CommandResult> ListManagedAccounts(AwsMgnListManagedAccountsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnListManagedAccountsOptions(), token);
    }

    public async Task<CommandResult> ListSourceServerActions(AwsMgnListSourceServerActionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsMgnListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTemplateActions(AwsMgnListTemplateActionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListWaves(AwsMgnListWavesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMgnListWavesOptions(), token);
    }

    public async Task<CommandResult> MarkAsArchived(AwsMgnMarkAsArchivedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PauseReplication(AwsMgnPauseReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutSourceServerAction(AwsMgnPutSourceServerActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutTemplateAction(AwsMgnPutTemplateActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveSourceServerAction(AwsMgnRemoveSourceServerActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTemplateAction(AwsMgnRemoveTemplateActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResumeReplication(AwsMgnResumeReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RetryDataReplication(AwsMgnRetryDataReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartCutover(AwsMgnStartCutoverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartExport(AwsMgnStartExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartImport(AwsMgnStartImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartReplication(AwsMgnStartReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartTest(AwsMgnStartTestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopReplication(AwsMgnStopReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsMgnTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TerminateTargetInstances(AwsMgnTerminateTargetInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnarchiveApplication(AwsMgnUnarchiveApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnarchiveWave(AwsMgnUnarchiveWaveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsMgnUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApplication(AwsMgnUpdateApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConnector(AwsMgnUpdateConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLaunchConfiguration(AwsMgnUpdateLaunchConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLaunchConfigurationTemplate(AwsMgnUpdateLaunchConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateReplicationConfiguration(AwsMgnUpdateReplicationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateReplicationConfigurationTemplate(AwsMgnUpdateReplicationConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSourceServer(AwsMgnUpdateSourceServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSourceServerReplicationType(AwsMgnUpdateSourceServerReplicationTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWave(AwsMgnUpdateWaveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}