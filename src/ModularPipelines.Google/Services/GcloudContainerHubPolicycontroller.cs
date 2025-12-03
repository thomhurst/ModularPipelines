using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub")]
public class GcloudContainerHubPolicycontroller
{
    public GcloudContainerHubPolicycontroller(
        GcloudContainerHubPolicycontrollerContent content,
        GcloudContainerHubPolicycontrollerDeployment deployment,
        ICommand internalCommand
    )
    {
        Content = content;
        Deployment = deployment;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerHubPolicycontrollerContent Content { get; }

    public GcloudContainerHubPolicycontrollerDeployment Deployment { get; }

    public async Task<CommandResult> Describe(GcloudContainerHubPolicycontrollerDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubPolicycontrollerDescribeOptions(), token);
    }

    public async Task<CommandResult> Detach(GcloudContainerHubPolicycontrollerDetachOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubPolicycontrollerDetachOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerHubPolicycontrollerDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubPolicycontrollerDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerHubPolicycontrollerEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubPolicycontrollerEnableOptions(), token);
    }

    public async Task<CommandResult> Suspend(GcloudContainerHubPolicycontrollerSuspendOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubPolicycontrollerSuspendOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudContainerHubPolicycontrollerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubPolicycontrollerUpdateOptions(), token);
    }
}