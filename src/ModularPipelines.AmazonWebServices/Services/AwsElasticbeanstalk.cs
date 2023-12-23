using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> AbortEnvironmentUpdate(AwsElasticbeanstalkAbortEnvironmentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkAbortEnvironmentUpdateOptions(), token);
    }

    public async Task<CommandResult> ApplyEnvironmentManagedAction(AwsElasticbeanstalkApplyEnvironmentManagedActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateEnvironmentOperationsRole(AwsElasticbeanstalkAssociateEnvironmentOperationsRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckDnsAvailability(AwsElasticbeanstalkCheckDnsAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ComposeEnvironments(AwsElasticbeanstalkComposeEnvironmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkComposeEnvironmentsOptions(), token);
    }

    public async Task<CommandResult> CreateApplication(AwsElasticbeanstalkCreateApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateApplicationVersion(AwsElasticbeanstalkCreateApplicationVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfigurationTemplate(AwsElasticbeanstalkCreateConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEnvironment(AwsElasticbeanstalkCreateEnvironmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePlatformVersion(AwsElasticbeanstalkCreatePlatformVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStorageLocation(AwsElasticbeanstalkCreateStorageLocationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkCreateStorageLocationOptions(), token);
    }

    public async Task<CommandResult> DeleteApplication(AwsElasticbeanstalkDeleteApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApplicationVersion(AwsElasticbeanstalkDeleteApplicationVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationTemplate(AwsElasticbeanstalkDeleteConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEnvironmentConfiguration(AwsElasticbeanstalkDeleteEnvironmentConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePlatformVersion(AwsElasticbeanstalkDeletePlatformVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDeletePlatformVersionOptions(), token);
    }

    public async Task<CommandResult> DescribeAccountAttributes(AwsElasticbeanstalkDescribeAccountAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeAccountAttributesOptions(), token);
    }

    public async Task<CommandResult> DescribeApplicationVersions(AwsElasticbeanstalkDescribeApplicationVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeApplicationVersionsOptions(), token);
    }

    public async Task<CommandResult> DescribeApplications(AwsElasticbeanstalkDescribeApplicationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeApplicationsOptions(), token);
    }

    public async Task<CommandResult> DescribeConfigurationOptions(AwsElasticbeanstalkDescribeConfigurationOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeConfigurationOptionsOptions(), token);
    }

    public async Task<CommandResult> DescribeConfigurationSettings(AwsElasticbeanstalkDescribeConfigurationSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEnvironmentHealth(AwsElasticbeanstalkDescribeEnvironmentHealthOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEnvironmentHealthOptions(), token);
    }

    public async Task<CommandResult> DescribeEnvironmentManagedActionHistory(AwsElasticbeanstalkDescribeEnvironmentManagedActionHistoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEnvironmentManagedActionHistoryOptions(), token);
    }

    public async Task<CommandResult> DescribeEnvironmentManagedActions(AwsElasticbeanstalkDescribeEnvironmentManagedActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEnvironmentManagedActionsOptions(), token);
    }

    public async Task<CommandResult> DescribeEnvironmentResources(AwsElasticbeanstalkDescribeEnvironmentResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEnvironmentResourcesOptions(), token);
    }

    public async Task<CommandResult> DescribeEnvironments(AwsElasticbeanstalkDescribeEnvironmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEnvironmentsOptions(), token);
    }

    public async Task<CommandResult> DescribeEvents(AwsElasticbeanstalkDescribeEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeEventsOptions(), token);
    }

    public async Task<CommandResult> DescribeInstancesHealth(AwsElasticbeanstalkDescribeInstancesHealthOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribeInstancesHealthOptions(), token);
    }

    public async Task<CommandResult> DescribePlatformVersion(AwsElasticbeanstalkDescribePlatformVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkDescribePlatformVersionOptions(), token);
    }

    public async Task<CommandResult> DisassociateEnvironmentOperationsRole(AwsElasticbeanstalkDisassociateEnvironmentOperationsRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAvailableSolutionStacks(AwsElasticbeanstalkListAvailableSolutionStacksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkListAvailableSolutionStacksOptions(), token);
    }

    public async Task<CommandResult> ListPlatformBranches(AwsElasticbeanstalkListPlatformBranchesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkListPlatformBranchesOptions(), token);
    }

    public async Task<CommandResult> ListPlatformVersions(AwsElasticbeanstalkListPlatformVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkListPlatformVersionsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsElasticbeanstalkListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebuildEnvironment(AwsElasticbeanstalkRebuildEnvironmentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkRebuildEnvironmentOptions(), token);
    }

    public async Task<CommandResult> RequestEnvironmentInfo(AwsElasticbeanstalkRequestEnvironmentInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestartAppServer(AwsElasticbeanstalkRestartAppServerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkRestartAppServerOptions(), token);
    }

    public async Task<CommandResult> RetrieveEnvironmentInfo(AwsElasticbeanstalkRetrieveEnvironmentInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SwapEnvironmentCnames(AwsElasticbeanstalkSwapEnvironmentCnamesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkSwapEnvironmentCnamesOptions(), token);
    }

    public async Task<CommandResult> TerminateEnvironment(AwsElasticbeanstalkTerminateEnvironmentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkTerminateEnvironmentOptions(), token);
    }

    public async Task<CommandResult> UpdateApplication(AwsElasticbeanstalkUpdateApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApplicationResourceLifecycle(AwsElasticbeanstalkUpdateApplicationResourceLifecycleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApplicationVersion(AwsElasticbeanstalkUpdateApplicationVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfigurationTemplate(AwsElasticbeanstalkUpdateConfigurationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEnvironment(AwsElasticbeanstalkUpdateEnvironmentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElasticbeanstalkUpdateEnvironmentOptions(), token);
    }

    public async Task<CommandResult> UpdateTagsForResource(AwsElasticbeanstalkUpdateTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ValidateConfigurationSettings(AwsElasticbeanstalkValidateConfigurationSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}