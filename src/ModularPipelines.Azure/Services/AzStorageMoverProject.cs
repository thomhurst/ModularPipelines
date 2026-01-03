using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage-mover")]
public class AzStorageMoverProject
{
    public AzStorageMoverProject(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzStorageMoverProjectCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzStorageMoverProjectDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverProjectDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzStorageMoverProjectListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzStorageMoverProjectShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverProjectShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzStorageMoverProjectUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverProjectUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzStorageMoverProjectWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverProjectWaitOptions(), cancellationToken: token);
    }
}