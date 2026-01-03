using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("desktopvirtualization")]
public class AzDesktopvirtualizationWorkspace
{
    public AzDesktopvirtualizationWorkspace(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDesktopvirtualizationWorkspaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDesktopvirtualizationWorkspaceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationWorkspaceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDesktopvirtualizationWorkspaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationWorkspaceListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDesktopvirtualizationWorkspaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationWorkspaceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDesktopvirtualizationWorkspaceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationWorkspaceUpdateOptions(), cancellationToken: token);
    }
}