using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet")]
public class GcloudContainerFleetPolicycontroller
{
    public GcloudContainerFleetPolicycontroller(
        GcloudContainerFleetPolicycontrollerContent content,
        GcloudContainerFleetPolicycontrollerDeployment deployment,
        ICommand internalCommand
    )
    {
        Content = content;
        Deployment = deployment;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerFleetPolicycontrollerContent Content { get; }

    public GcloudContainerFleetPolicycontrollerDeployment Deployment { get; }

    public async Task<CommandResult> Describe(GcloudContainerFleetPolicycontrollerDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetPolicycontrollerDescribeOptions(), token);
    }

    public async Task<CommandResult> Detach(GcloudContainerFleetPolicycontrollerDetachOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetPolicycontrollerDetachOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerFleetPolicycontrollerDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetPolicycontrollerDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerFleetPolicycontrollerEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetPolicycontrollerEnableOptions(), token);
    }

    public async Task<CommandResult> Suspend(GcloudContainerFleetPolicycontrollerSuspendOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetPolicycontrollerSuspendOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudContainerFleetPolicycontrollerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetPolicycontrollerUpdateOptions(), token);
    }
}