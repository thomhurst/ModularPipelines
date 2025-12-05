using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub")]
public class GcloudContainerHubClusterupgrade
{
    public GcloudContainerHubClusterupgrade(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(GcloudContainerHubClusterupgradeCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubClusterupgradeCreateOptions(), token);
    }

    public async Task<CommandResult> Describe(GcloudContainerHubClusterupgradeDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubClusterupgradeDescribeOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudContainerHubClusterupgradeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubClusterupgradeUpdateOptions(), token);
    }
}