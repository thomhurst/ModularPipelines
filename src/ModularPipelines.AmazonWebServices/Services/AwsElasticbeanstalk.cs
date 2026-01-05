using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsElasticbeanstalk
{
    public AwsElasticbeanstalk(
        AwsElasticbeanstalkWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsElasticbeanstalkWait Wait { get; }

    public async Task<CommandResult> AbortEnvironmentUpdate(AwsElasticbeanstalkAbortEnvironmentUpdateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkAbortEnvironmentUpdateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ApplyEnvironmentManagedAction(AwsElasticbeanstalkApplyEnvironmentManagedActionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateEnvironmentOperationsRole(AwsElasticbeanstalkAssociateEnvironmentOperationsRoleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CheckDnsAvailability(AwsElasticbeanstalkCheckDnsAvailabilityOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ComposeEnvironments(AwsElasticbeanstalkComposeEnvironmentsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkComposeEnvironmentsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateApplication(AwsElasticbeanstalkCreateApplicationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateApplicationVersion(AwsElasticbeanstalkCreateApplicationVersionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateConfigurationTemplate(AwsElasticbeanstalkCreateConfigurationTemplateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateEnvironment(AwsElasticbeanstalkCreateEnvironmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreatePlatformVersion(AwsElasticbeanstalkCreatePlatformVersionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateStorageLocation(AwsElasticbeanstalkCreateStorageLocationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkCreateStorageLocationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteApplication(AwsElasticbeanstalkDeleteApplicationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteApplicationVersion(AwsElasticbeanstalkDeleteApplicationVersionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConfigurationTemplate(AwsElasticbeanstalkDeleteConfigurationTemplateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteEnvironmentConfiguration(AwsElasticbeanstalkDeleteEnvironmentConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeletePlatformVersion(AwsElasticbeanstalkDeletePlatformVersionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDeletePlatformVersionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAccountAttributes(AwsElasticbeanstalkDescribeAccountAttributesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeAccountAttributesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeApplicationVersions(AwsElasticbeanstalkDescribeApplicationVersionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeApplicationVersionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeApplications(AwsElasticbeanstalkDescribeApplicationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeApplicationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConfigurationOptions(AwsElasticbeanstalkDescribeConfigurationOptionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeConfigurationOptionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConfigurationSettings(AwsElasticbeanstalkDescribeConfigurationSettingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEnvironmentHealth(AwsElasticbeanstalkDescribeEnvironmentHealthOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEnvironmentHealthOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEnvironmentManagedActionHistory(AwsElasticbeanstalkDescribeEnvironmentManagedActionHistoryOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEnvironmentManagedActionHistoryOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEnvironmentManagedActions(AwsElasticbeanstalkDescribeEnvironmentManagedActionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEnvironmentManagedActionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEnvironmentResources(AwsElasticbeanstalkDescribeEnvironmentResourcesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEnvironmentResourcesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEnvironments(AwsElasticbeanstalkDescribeEnvironmentsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEnvironmentsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEvents(AwsElasticbeanstalkDescribeEventsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEventsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeInstancesHealth(AwsElasticbeanstalkDescribeInstancesHealthOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeInstancesHealthOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePlatformVersion(AwsElasticbeanstalkDescribePlatformVersionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribePlatformVersionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateEnvironmentOperationsRole(AwsElasticbeanstalkDisassociateEnvironmentOperationsRoleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAvailableSolutionStacks(AwsElasticbeanstalkListAvailableSolutionStacksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkListAvailableSolutionStacksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPlatformBranches(AwsElasticbeanstalkListPlatformBranchesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkListPlatformBranchesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPlatformVersions(AwsElasticbeanstalkListPlatformVersionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkListPlatformVersionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsElasticbeanstalkListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RebuildEnvironment(AwsElasticbeanstalkRebuildEnvironmentOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkRebuildEnvironmentOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RequestEnvironmentInfo(AwsElasticbeanstalkRequestEnvironmentInfoOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RestartAppServer(AwsElasticbeanstalkRestartAppServerOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkRestartAppServerOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RetrieveEnvironmentInfo(AwsElasticbeanstalkRetrieveEnvironmentInfoOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SwapEnvironmentCnames(AwsElasticbeanstalkSwapEnvironmentCnamesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkSwapEnvironmentCnamesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TerminateEnvironment(AwsElasticbeanstalkTerminateEnvironmentOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkTerminateEnvironmentOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateApplication(AwsElasticbeanstalkUpdateApplicationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateApplicationResourceLifecycle(AwsElasticbeanstalkUpdateApplicationResourceLifecycleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateApplicationVersion(AwsElasticbeanstalkUpdateApplicationVersionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateConfigurationTemplate(AwsElasticbeanstalkUpdateConfigurationTemplateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateEnvironment(AwsElasticbeanstalkUpdateEnvironmentOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkUpdateEnvironmentOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateTagsForResource(AwsElasticbeanstalkUpdateTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ValidateConfigurationSettings(AwsElasticbeanstalkValidateConfigurationSettingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}