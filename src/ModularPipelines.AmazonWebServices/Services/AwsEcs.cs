using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsEcs
{
    public AwsEcs(
        AwsEcsWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsEcsWait Wait { get; }

    public async Task<CommandResult> CreateCapacityProvider(AwsEcsCreateCapacityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCluster(AwsEcsCreateClusterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsCreateClusterOptions(), token);
    }

    public async Task<CommandResult> CreateService(AwsEcsCreateServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTaskSet(AwsEcsCreateTaskSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAccountSetting(AwsEcsDeleteAccountSettingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAttributes(AwsEcsDeleteAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCapacityProvider(AwsEcsDeleteCapacityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCluster(AwsEcsDeleteClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteService(AwsEcsDeleteServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTaskDefinitions(AwsEcsDeleteTaskDefinitionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTaskSet(AwsEcsDeleteTaskSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deploy(AwsEcsDeployOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterContainerInstance(AwsEcsDeregisterContainerInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterTaskDefinition(AwsEcsDeregisterTaskDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCapacityProviders(AwsEcsDescribeCapacityProvidersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsDescribeCapacityProvidersOptions(), token);
    }

    public async Task<CommandResult> DescribeClusters(AwsEcsDescribeClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsDescribeClustersOptions(), token);
    }

    public async Task<CommandResult> DescribeContainerInstances(AwsEcsDescribeContainerInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeServices(AwsEcsDescribeServicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTaskDefinition(AwsEcsDescribeTaskDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTaskSets(AwsEcsDescribeTaskSetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTasks(AwsEcsDescribeTasksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DiscoverPollEndpoint(AwsEcsDiscoverPollEndpointOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsDiscoverPollEndpointOptions(), token);
    }

    public async Task<CommandResult> ExecuteCommand(AwsEcsExecuteCommandOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTaskProtection(AwsEcsGetTaskProtectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccountSettings(AwsEcsListAccountSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsListAccountSettingsOptions(), token);
    }

    public async Task<CommandResult> ListAttributes(AwsEcsListAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListClusters(AwsEcsListClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsListClustersOptions(), token);
    }

    public async Task<CommandResult> ListContainerInstances(AwsEcsListContainerInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsListContainerInstancesOptions(), token);
    }

    public async Task<CommandResult> ListServices(AwsEcsListServicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsListServicesOptions(), token);
    }

    public async Task<CommandResult> ListServicesByNamespace(AwsEcsListServicesByNamespaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsEcsListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTaskDefinitionFamilies(AwsEcsListTaskDefinitionFamiliesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsListTaskDefinitionFamiliesOptions(), token);
    }

    public async Task<CommandResult> ListTaskDefinitions(AwsEcsListTaskDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsListTaskDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListTasks(AwsEcsListTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsListTasksOptions(), token);
    }

    public async Task<CommandResult> PutAccountSetting(AwsEcsPutAccountSettingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAccountSettingDefault(AwsEcsPutAccountSettingDefaultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAttributes(AwsEcsPutAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutClusterCapacityProviders(AwsEcsPutClusterCapacityProvidersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterContainerInstance(AwsEcsRegisterContainerInstanceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsRegisterContainerInstanceOptions(), token);
    }

    public async Task<CommandResult> RegisterTaskDefinition(AwsEcsRegisterTaskDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RunTask(AwsEcsRunTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartTask(AwsEcsStartTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopTask(AwsEcsStopTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SubmitAttachmentStateChanges(AwsEcsSubmitAttachmentStateChangesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SubmitContainerStateChange(AwsEcsSubmitContainerStateChangeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsSubmitContainerStateChangeOptions(), token);
    }

    public async Task<CommandResult> SubmitTaskStateChange(AwsEcsSubmitTaskStateChangeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEcsSubmitTaskStateChangeOptions(), token);
    }

    public async Task<CommandResult> TagResource(AwsEcsTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsEcsUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCapacityProvider(AwsEcsUpdateCapacityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCluster(AwsEcsUpdateClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateClusterSettings(AwsEcsUpdateClusterSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateContainerAgent(AwsEcsUpdateContainerAgentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateContainerInstancesState(AwsEcsUpdateContainerInstancesStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateService(AwsEcsUpdateServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateServicePrimaryTaskSet(AwsEcsUpdateServicePrimaryTaskSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTaskProtection(AwsEcsUpdateTaskProtectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTaskSet(AwsEcsUpdateTaskSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}