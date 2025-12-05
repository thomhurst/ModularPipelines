using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters")]
public class GcloudAccessContextManagerPerimetersDryRun
{
    public GcloudAccessContextManagerPerimetersDryRun(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(GcloudAccessContextManagerPerimetersDryRunCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudAccessContextManagerPerimetersDryRunDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudAccessContextManagerPerimetersDryRunDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Drop(GcloudAccessContextManagerPerimetersDryRunDropOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Enforce(GcloudAccessContextManagerPerimetersDryRunEnforceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnforceAll(GcloudAccessContextManagerPerimetersDryRunEnforceAllOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAccessContextManagerPerimetersDryRunEnforceAllOptions(), token);
    }

    public async Task<CommandResult> List(GcloudAccessContextManagerPerimetersDryRunListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAccessContextManagerPerimetersDryRunListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudAccessContextManagerPerimetersDryRunUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}