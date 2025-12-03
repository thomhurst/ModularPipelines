using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet")]
public class GcloudContainerFleetClusterupgrade
{
    public GcloudContainerFleetClusterupgrade(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(GcloudContainerFleetClusterupgradeCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetClusterupgradeCreateOptions(), token);
    }

    public async Task<CommandResult> Describe(GcloudContainerFleetClusterupgradeDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetClusterupgradeDescribeOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudContainerFleetClusterupgradeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetClusterupgradeUpdateOptions(), token);
    }
}