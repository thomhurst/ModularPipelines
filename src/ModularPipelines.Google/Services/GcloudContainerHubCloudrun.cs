using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub")]
public class GcloudContainerHubCloudrun
{
    public GcloudContainerHubCloudrun(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Apply(GcloudContainerHubCloudrunApplyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudContainerHubCloudrunDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubCloudrunDescribeOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerHubCloudrunDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubCloudrunDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerHubCloudrunEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubCloudrunEnableOptions(), token);
    }
}