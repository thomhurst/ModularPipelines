using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSms
{
    public AwsSms(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateApp(AwsSmsCreateAppOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsCreateAppOptions(), token);
    }

    public async Task<CommandResult> CreateReplicationJob(AwsSmsCreateReplicationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApp(AwsSmsDeleteAppOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsDeleteAppOptions(), token);
    }

    public async Task<CommandResult> DeleteAppLaunchConfiguration(AwsSmsDeleteAppLaunchConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsDeleteAppLaunchConfigurationOptions(), token);
    }

    public async Task<CommandResult> DeleteAppReplicationConfiguration(AwsSmsDeleteAppReplicationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsDeleteAppReplicationConfigurationOptions(), token);
    }

    public async Task<CommandResult> DeleteAppValidationConfiguration(AwsSmsDeleteAppValidationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReplicationJob(AwsSmsDeleteReplicationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteServerCatalog(AwsSmsDeleteServerCatalogOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsDeleteServerCatalogOptions(), token);
    }

    public async Task<CommandResult> DisassociateConnector(AwsSmsDisassociateConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateChangeSet(AwsSmsGenerateChangeSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGenerateChangeSetOptions(), token);
    }

    public async Task<CommandResult> GenerateTemplate(AwsSmsGenerateTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGenerateTemplateOptions(), token);
    }

    public async Task<CommandResult> GetApp(AwsSmsGetAppOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetAppOptions(), token);
    }

    public async Task<CommandResult> GetAppLaunchConfiguration(AwsSmsGetAppLaunchConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetAppLaunchConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetAppReplicationConfiguration(AwsSmsGetAppReplicationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetAppReplicationConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetAppValidationConfiguration(AwsSmsGetAppValidationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAppValidationOutput(AwsSmsGetAppValidationOutputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnectors(AwsSmsGetConnectorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetConnectorsOptions(), token);
    }

    public async Task<CommandResult> GetReplicationJobs(AwsSmsGetReplicationJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetReplicationJobsOptions(), token);
    }

    public async Task<CommandResult> GetReplicationRuns(AwsSmsGetReplicationRunsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServers(AwsSmsGetServersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetServersOptions(), token);
    }

    public async Task<CommandResult> ImportAppCatalog(AwsSmsImportAppCatalogOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsImportAppCatalogOptions(), token);
    }

    public async Task<CommandResult> ImportServerCatalog(AwsSmsImportServerCatalogOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsImportServerCatalogOptions(), token);
    }

    public async Task<CommandResult> LaunchApp(AwsSmsLaunchAppOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsLaunchAppOptions(), token);
    }

    public async Task<CommandResult> ListApps(AwsSmsListAppsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsListAppsOptions(), token);
    }

    public async Task<CommandResult> NotifyAppValidationOutput(AwsSmsNotifyAppValidationOutputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAppLaunchConfiguration(AwsSmsPutAppLaunchConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsPutAppLaunchConfigurationOptions(), token);
    }

    public async Task<CommandResult> PutAppReplicationConfiguration(AwsSmsPutAppReplicationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsPutAppReplicationConfigurationOptions(), token);
    }

    public async Task<CommandResult> PutAppValidationConfiguration(AwsSmsPutAppValidationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartAppReplication(AwsSmsStartAppReplicationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsStartAppReplicationOptions(), token);
    }

    public async Task<CommandResult> StartOnDemandAppReplication(AwsSmsStartOnDemandAppReplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartOnDemandReplicationRun(AwsSmsStartOnDemandReplicationRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopAppReplication(AwsSmsStopAppReplicationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsStopAppReplicationOptions(), token);
    }

    public async Task<CommandResult> TerminateApp(AwsSmsTerminateAppOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsTerminateAppOptions(), token);
    }

    public async Task<CommandResult> UpdateApp(AwsSmsUpdateAppOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsUpdateAppOptions(), token);
    }

    public async Task<CommandResult> UpdateReplicationJob(AwsSmsUpdateReplicationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}