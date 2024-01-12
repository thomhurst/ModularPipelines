using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-groups")]
public class GcloudComputeInstanceGroupsManaged
{
    public GcloudComputeInstanceGroupsManaged(
        GcloudComputeInstanceGroupsManagedInstanceConfigs instanceConfigs,
        GcloudComputeInstanceGroupsManagedRollingAction rollingAction,
        ICommand internalCommand
    )
    {
        InstanceConfigs = instanceConfigs;
        RollingAction = rollingAction;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeInstanceGroupsManagedInstanceConfigs InstanceConfigs { get; }

    public GcloudComputeInstanceGroupsManagedRollingAction RollingAction { get; }

    public async Task<CommandResult> AbandonInstances(GcloudComputeInstanceGroupsManagedAbandonInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudComputeInstanceGroupsManagedCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInstance(GcloudComputeInstanceGroupsManagedCreateInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudComputeInstanceGroupsManagedDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstances(GcloudComputeInstanceGroupsManagedDeleteInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComputeInstanceGroupsManagedDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInstance(GcloudComputeInstanceGroupsManagedDescribeInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNamedPorts(GcloudComputeInstanceGroupsManagedGetNamedPortsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeInstanceGroupsManagedListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListErrors(GcloudComputeInstanceGroupsManagedListErrorsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInstances(GcloudComputeInstanceGroupsManagedListInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RecreateInstances(GcloudComputeInstanceGroupsManagedRecreateInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Resize(GcloudComputeInstanceGroupsManagedResizeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetAutoscaling(GcloudComputeInstanceGroupsManagedSetAutoscalingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetInstanceTemplate(GcloudComputeInstanceGroupsManagedSetInstanceTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetNamedPorts(GcloudComputeInstanceGroupsManagedSetNamedPortsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetTargetPools(GcloudComputeInstanceGroupsManagedSetTargetPoolsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopAutoscaling(GcloudComputeInstanceGroupsManagedStopAutoscalingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudComputeInstanceGroupsManagedUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAutoscaling(GcloudComputeInstanceGroupsManagedUpdateAutoscalingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInstances(GcloudComputeInstanceGroupsManagedUpdateInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> WaitUntil(GcloudComputeInstanceGroupsManagedWaitUntilOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}