using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml")]
public class AzMlData
{
    public AzMlData(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Archive(AzMlDataArchiveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzMlDataCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDataCreateOptions(), token);
    }

    public async Task<CommandResult> Import(AzMlDataImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMlDataListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlDataListOptions(), token);
    }

    public async Task<CommandResult> ListMaterializationStatus(AzMlDataListMaterializationStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzMlDataRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Share(AzMlDataShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMlDataShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzMlDataUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}