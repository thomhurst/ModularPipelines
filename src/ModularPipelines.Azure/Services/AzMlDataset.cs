using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml")]
public class AzMlDataset
{
    public AzMlDataset(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Archive(AzMlDatasetArchiveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDatasetArchiveOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Deprecate(AzMlDatasetDeprecateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> List(AzMlDatasetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDatasetListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Reactivate(AzMlDatasetReactivateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDatasetReactivateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Register(AzMlDatasetRegisterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDatasetRegisterOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMlDatasetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDatasetShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Unregister(AzMlDatasetUnregisterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDatasetUnregisterOptions(), cancellationToken: token);
    }
}