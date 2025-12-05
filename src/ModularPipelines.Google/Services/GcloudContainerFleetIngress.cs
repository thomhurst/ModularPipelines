using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet")]
public class GcloudContainerFleetIngress
{
    public GcloudContainerFleetIngress(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudContainerFleetIngressDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetIngressDescribeOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerFleetIngressDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetIngressDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerFleetIngressEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetIngressEnableOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudContainerFleetIngressUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetIngressUpdateOptions(), token);
    }
}