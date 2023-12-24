using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsDeploy
{
    public AwsDeploy(
        AwsDeployWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsDeployWait Wait { get; }

    public async Task<CommandResult> AddTagsToOnPremisesInstances(AwsDeployAddTagsToOnPremisesInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetApplicationRevisions(AwsDeployBatchGetApplicationRevisionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetApplications(AwsDeployBatchGetApplicationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetDeploymentGroups(AwsDeployBatchGetDeploymentGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetDeploymentTargets(AwsDeployBatchGetDeploymentTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetDeployments(AwsDeployBatchGetDeploymentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetOnPremisesInstances(AwsDeployBatchGetOnPremisesInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContinueDeployment(AwsDeployContinueDeploymentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDeployContinueDeploymentOptions(), token);
    }

    public async Task<CommandResult> CreateApplication(AwsDeployCreateApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDeployment(AwsDeployCreateDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDeploymentConfig(AwsDeployCreateDeploymentConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDeploymentGroup(AwsDeployCreateDeploymentGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApplication(AwsDeployDeleteApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDeploymentConfig(AwsDeployDeleteDeploymentConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDeploymentGroup(AwsDeployDeleteDeploymentGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGitHubAccountToken(AwsDeployDeleteGitHubAccountTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDeployDeleteGitHubAccountTokenOptions(), token);
    }

    public async Task<CommandResult> DeleteResourcesByExternalId(AwsDeployDeleteResourcesByExternalIdOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDeployDeleteResourcesByExternalIdOptions(), token);
    }

    public async Task<CommandResult> Deregister(AwsDeployDeregisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterOnPremisesInstance(AwsDeployDeregisterOnPremisesInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetApplication(AwsDeployGetApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetApplicationRevision(AwsDeployGetApplicationRevisionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeployment(AwsDeployGetDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeploymentConfig(AwsDeployGetDeploymentConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeploymentGroup(AwsDeployGetDeploymentGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeploymentTarget(AwsDeployGetDeploymentTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOnPremisesInstance(AwsDeployGetOnPremisesInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Install(AwsDeployInstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListApplicationRevisions(AwsDeployListApplicationRevisionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListApplications(AwsDeployListApplicationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDeployListApplicationsOptions(), token);
    }

    public async Task<CommandResult> ListDeploymentConfigs(AwsDeployListDeploymentConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDeployListDeploymentConfigsOptions(), token);
    }

    public async Task<CommandResult> ListDeploymentGroups(AwsDeployListDeploymentGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDeploymentTargets(AwsDeployListDeploymentTargetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDeployments(AwsDeployListDeploymentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDeployListDeploymentsOptions(), token);
    }

    public async Task<CommandResult> ListGitHubAccountTokenNames(AwsDeployListGitHubAccountTokenNamesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDeployListGitHubAccountTokenNamesOptions(), token);
    }

    public async Task<CommandResult> ListOnPremisesInstances(AwsDeployListOnPremisesInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDeployListOnPremisesInstancesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsDeployListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Push(AwsDeployPushOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutLifecycleEventHookExecutionStatus(AwsDeployPutLifecycleEventHookExecutionStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDeployPutLifecycleEventHookExecutionStatusOptions(), token);
    }

    public async Task<CommandResult> Register(AwsDeployRegisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterApplicationRevision(AwsDeployRegisterApplicationRevisionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterOnPremisesInstance(AwsDeployRegisterOnPremisesInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTagsFromOnPremisesInstances(AwsDeployRemoveTagsFromOnPremisesInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopDeployment(AwsDeployStopDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsDeployTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Uninstall(AwsDeployUninstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDeployUninstallOptions(), token);
    }

    public async Task<CommandResult> UntagResource(AwsDeployUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApplication(AwsDeployUpdateApplicationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDeployUpdateApplicationOptions(), token);
    }

    public async Task<CommandResult> UpdateDeploymentGroup(AwsDeployUpdateDeploymentGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}