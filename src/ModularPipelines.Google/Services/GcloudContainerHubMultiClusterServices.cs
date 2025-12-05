using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub")]
public class GcloudContainerHubMultiClusterServices
{
    public GcloudContainerHubMultiClusterServices(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudContainerHubMultiClusterServicesDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubMultiClusterServicesDescribeOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerHubMultiClusterServicesDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubMultiClusterServicesDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerHubMultiClusterServicesEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubMultiClusterServicesEnableOptions(), token);
    }
}