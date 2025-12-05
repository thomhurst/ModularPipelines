using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub")]
public class GcloudContainerHubIdentityService
{
    public GcloudContainerHubIdentityService(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Apply(GcloudContainerHubIdentityServiceApplyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudContainerHubIdentityServiceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubIdentityServiceDeleteOptions(), token);
    }

    public async Task<CommandResult> Describe(GcloudContainerHubIdentityServiceDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubIdentityServiceDescribeOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerHubIdentityServiceDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubIdentityServiceDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerHubIdentityServiceEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubIdentityServiceEnableOptions(), token);
    }
}