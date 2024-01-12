using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudComponents
{
    public GcloudComponents(
        GcloudComponentsRepositories repositories,
        ICommand internalCommand
    )
    {
        Repositories = repositories;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComponentsRepositories Repositories { get; }

    public async Task<CommandResult> Install(GcloudComponentsInstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComponentsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComponentsListOptions(), token);
    }

    public async Task<CommandResult> Reinstall(GcloudComponentsReinstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComponentsReinstallOptions(), token);
    }

    public async Task<CommandResult> Remove(GcloudComponentsRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudComponentsUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComponentsUpdateOptions(), token);
    }
}