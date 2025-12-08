using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("builds")]
public class GcloudBuildsTriggers
{
    public GcloudBuildsTriggers(
        GcloudBuildsTriggersCreate create,
        GcloudBuildsTriggersUpdate update,
        ICommand internalCommand
    )
    {
        Create = create;
        Update = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudBuildsTriggersCreate Create { get; }

    public GcloudBuildsTriggersUpdate Update { get; }

    public async Task<CommandResult> Delete(GcloudBuildsTriggersDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudBuildsTriggersDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(GcloudBuildsTriggersImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudBuildsTriggersListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudBuildsTriggersListOptions(), token);
    }

    public async Task<CommandResult> Run(GcloudBuildsTriggersRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}