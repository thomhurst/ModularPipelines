using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> CreateApp(AwsSmsCreateAppOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsCreateAppOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateReplicationJob(AwsSmsCreateReplicationJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteApp(AwsSmsDeleteAppOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsDeleteAppOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteAppLaunchConfiguration(AwsSmsDeleteAppLaunchConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsDeleteAppLaunchConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteAppReplicationConfiguration(AwsSmsDeleteAppReplicationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsDeleteAppReplicationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteAppValidationConfiguration(AwsSmsDeleteAppValidationConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteReplicationJob(AwsSmsDeleteReplicationJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteServerCatalog(AwsSmsDeleteServerCatalogOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsDeleteServerCatalogOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateConnector(AwsSmsDisassociateConnectorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GenerateChangeSet(AwsSmsGenerateChangeSetOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGenerateChangeSetOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GenerateTemplate(AwsSmsGenerateTemplateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGenerateTemplateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetApp(AwsSmsGetAppOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetAppOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAppLaunchConfiguration(AwsSmsGetAppLaunchConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetAppLaunchConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAppReplicationConfiguration(AwsSmsGetAppReplicationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetAppReplicationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAppValidationConfiguration(AwsSmsGetAppValidationConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAppValidationOutput(AwsSmsGetAppValidationOutputOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetConnectors(AwsSmsGetConnectorsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetConnectorsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetReplicationJobs(AwsSmsGetReplicationJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetReplicationJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetReplicationRuns(AwsSmsGetReplicationRunsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetServers(AwsSmsGetServersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsGetServersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ImportAppCatalog(AwsSmsImportAppCatalogOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsImportAppCatalogOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ImportServerCatalog(AwsSmsImportServerCatalogOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsImportServerCatalogOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> LaunchApp(AwsSmsLaunchAppOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsLaunchAppOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListApps(AwsSmsListAppsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsListAppsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> NotifyAppValidationOutput(AwsSmsNotifyAppValidationOutputOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutAppLaunchConfiguration(AwsSmsPutAppLaunchConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsPutAppLaunchConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutAppReplicationConfiguration(AwsSmsPutAppReplicationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsPutAppReplicationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutAppValidationConfiguration(AwsSmsPutAppValidationConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartAppReplication(AwsSmsStartAppReplicationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsStartAppReplicationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartOnDemandAppReplication(AwsSmsStartOnDemandAppReplicationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartOnDemandReplicationRun(AwsSmsStartOnDemandReplicationRunOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopAppReplication(AwsSmsStopAppReplicationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsStopAppReplicationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TerminateApp(AwsSmsTerminateAppOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsTerminateAppOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateApp(AwsSmsUpdateAppOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSmsUpdateAppOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateReplicationJob(AwsSmsUpdateReplicationJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}