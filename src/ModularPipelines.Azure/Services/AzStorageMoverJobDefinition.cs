using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage-mover")]
public class AzStorageMoverJobDefinition
{
    public AzStorageMoverJobDefinition(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzStorageMoverJobDefinitionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzStorageMoverJobDefinitionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverJobDefinitionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzStorageMoverJobDefinitionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzStorageMoverJobDefinitionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverJobDefinitionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StartJob(AzStorageMoverJobDefinitionStartJobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverJobDefinitionStartJobOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StopJob(AzStorageMoverJobDefinitionStopJobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverJobDefinitionStopJobOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzStorageMoverJobDefinitionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverJobDefinitionUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzStorageMoverJobDefinitionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageMoverJobDefinitionWaitOptions(), cancellationToken: token);
    }
}