using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub")]
public class GcloudContainerHubIngress
{
    public GcloudContainerHubIngress(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudContainerHubIngressDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubIngressDescribeOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerHubIngressDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubIngressDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerHubIngressEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubIngressEnableOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudContainerHubIngressUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubIngressUpdateOptions(), token);
    }
}