using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsAutoscaling
{
    public AwsAutoscaling(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AttachInstances(AwsAutoscalingAttachInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachLoadBalancerTargetGroups(AwsAutoscalingAttachLoadBalancerTargetGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachLoadBalancers(AwsAutoscalingAttachLoadBalancersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachTrafficSources(AwsAutoscalingAttachTrafficSourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeleteScheduledAction(AwsAutoscalingBatchDeleteScheduledActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchPutScheduledUpdateGroupAction(AwsAutoscalingBatchPutScheduledUpdateGroupActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelInstanceRefresh(AwsAutoscalingCancelInstanceRefreshOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CompleteLifecycleAction(AwsAutoscalingCompleteLifecycleActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAutoScalingGroup(AwsAutoscalingCreateAutoScalingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLaunchConfiguration(AwsAutoscalingCreateLaunchConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOrUpdateTags(AwsAutoscalingCreateOrUpdateTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAutoScalingGroup(AwsAutoscalingDeleteAutoScalingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLaunchConfiguration(AwsAutoscalingDeleteLaunchConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLifecycleHook(AwsAutoscalingDeleteLifecycleHookOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNotificationConfiguration(AwsAutoscalingDeleteNotificationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePolicy(AwsAutoscalingDeletePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteScheduledAction(AwsAutoscalingDeleteScheduledActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTags(AwsAutoscalingDeleteTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWarmPool(AwsAutoscalingDeleteWarmPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountLimits(AwsAutoscalingDescribeAccountLimitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeAccountLimitsOptions(), token);
    }

    public async Task<CommandResult> DescribeAdjustmentTypes(AwsAutoscalingDescribeAdjustmentTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeAdjustmentTypesOptions(), token);
    }

    public async Task<CommandResult> DescribeAutoScalingGroups(AwsAutoscalingDescribeAutoScalingGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeAutoScalingGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeAutoScalingInstances(AwsAutoscalingDescribeAutoScalingInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeAutoScalingInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeAutoScalingNotificationTypes(AwsAutoscalingDescribeAutoScalingNotificationTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeAutoScalingNotificationTypesOptions(), token);
    }

    public async Task<CommandResult> DescribeInstanceRefreshes(AwsAutoscalingDescribeInstanceRefreshesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLaunchConfigurations(AwsAutoscalingDescribeLaunchConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeLaunchConfigurationsOptions(), token);
    }

    public async Task<CommandResult> DescribeLifecycleHookTypes(AwsAutoscalingDescribeLifecycleHookTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeLifecycleHookTypesOptions(), token);
    }

    public async Task<CommandResult> DescribeLifecycleHooks(AwsAutoscalingDescribeLifecycleHooksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLoadBalancerTargetGroups(AwsAutoscalingDescribeLoadBalancerTargetGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLoadBalancers(AwsAutoscalingDescribeLoadBalancersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMetricCollectionTypes(AwsAutoscalingDescribeMetricCollectionTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeMetricCollectionTypesOptions(), token);
    }

    public async Task<CommandResult> DescribeNotificationConfigurations(AwsAutoscalingDescribeNotificationConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeNotificationConfigurationsOptions(), token);
    }

    public async Task<CommandResult> DescribePolicies(AwsAutoscalingDescribePoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribePoliciesOptions(), token);
    }

    public async Task<CommandResult> DescribeScalingActivities(AwsAutoscalingDescribeScalingActivitiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeScalingActivitiesOptions(), token);
    }

    public async Task<CommandResult> DescribeScalingProcessTypes(AwsAutoscalingDescribeScalingProcessTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeScalingProcessTypesOptions(), token);
    }

    public async Task<CommandResult> DescribeScheduledActions(AwsAutoscalingDescribeScheduledActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeScheduledActionsOptions(), token);
    }

    public async Task<CommandResult> DescribeTags(AwsAutoscalingDescribeTagsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeTagsOptions(), token);
    }

    public async Task<CommandResult> DescribeTerminationPolicyTypes(AwsAutoscalingDescribeTerminationPolicyTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAutoscalingDescribeTerminationPolicyTypesOptions(), token);
    }

    public async Task<CommandResult> DescribeTrafficSources(AwsAutoscalingDescribeTrafficSourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWarmPool(AwsAutoscalingDescribeWarmPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachInstances(AwsAutoscalingDetachInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachLoadBalancerTargetGroups(AwsAutoscalingDetachLoadBalancerTargetGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachLoadBalancers(AwsAutoscalingDetachLoadBalancersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachTrafficSources(AwsAutoscalingDetachTrafficSourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableMetricsCollection(AwsAutoscalingDisableMetricsCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableMetricsCollection(AwsAutoscalingEnableMetricsCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnterStandby(AwsAutoscalingEnterStandbyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecutePolicy(AwsAutoscalingExecutePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExitStandby(AwsAutoscalingExitStandbyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPredictiveScalingForecast(AwsAutoscalingGetPredictiveScalingForecastOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutLifecycleHook(AwsAutoscalingPutLifecycleHookOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutNotificationConfiguration(AwsAutoscalingPutNotificationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutScalingPolicy(AwsAutoscalingPutScalingPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutScheduledUpdateGroupAction(AwsAutoscalingPutScheduledUpdateGroupActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutWarmPool(AwsAutoscalingPutWarmPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RecordLifecycleActionHeartbeat(AwsAutoscalingRecordLifecycleActionHeartbeatOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResumeProcesses(AwsAutoscalingResumeProcessesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RollbackInstanceRefresh(AwsAutoscalingRollbackInstanceRefreshOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetDesiredCapacity(AwsAutoscalingSetDesiredCapacityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetInstanceHealth(AwsAutoscalingSetInstanceHealthOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetInstanceProtection(AwsAutoscalingSetInstanceProtectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartInstanceRefresh(AwsAutoscalingStartInstanceRefreshOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SuspendProcesses(AwsAutoscalingSuspendProcessesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TerminateInstanceInAutoScalingGroup(AwsAutoscalingTerminateInstanceInAutoScalingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAutoScalingGroup(AwsAutoscalingUpdateAutoScalingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}