using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager")]
public class GcloudAccessContextManagerPerimeters
{
    public GcloudAccessContextManagerPerimeters(
        GcloudAccessContextManagerPerimetersDryRun dryRun,
        ICommand internalCommand
    )
    {
        DryRun = dryRun;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudAccessContextManagerPerimetersDryRun DryRun { get; }

    public async Task<CommandResult> Create(GcloudAccessContextManagerPerimetersCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudAccessContextManagerPerimetersDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudAccessContextManagerPerimetersDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudAccessContextManagerPerimetersListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAccessContextManagerPerimetersListOptions(), token);
    }

    public async Task<CommandResult> ReplaceAll(GcloudAccessContextManagerPerimetersReplaceAllOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudAccessContextManagerPerimetersUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}