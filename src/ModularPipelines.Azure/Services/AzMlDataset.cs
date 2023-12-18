using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml")]
public class AzMlDataset
{
    public AzMlDataset(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Archive(AzMlDatasetArchiveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deprecate(AzMlDatasetDeprecateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMlDatasetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDatasetListOptions(), token);
    }

    public async Task<CommandResult> Reactivate(AzMlDatasetReactivateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDatasetReactivateOptions(), token);
    }

    public async Task<CommandResult> Register(AzMlDatasetRegisterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDatasetRegisterOptions(), token);
    }

    public async Task<CommandResult> Show(AzMlDatasetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDatasetShowOptions(), token);
    }

    public async Task<CommandResult> Unregister(AzMlDatasetUnregisterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDatasetUnregisterOptions(), token);
    }
}