using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzImportExport
{
    public AzImportExport(
        AzImportExportBitLockerKey bitLockerKey,
        AzImportExportLocation location,
        ICommand internalCommand
    )
    {
        BitLockerKey = bitLockerKey;
        Location = location;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzImportExportBitLockerKey BitLockerKey { get; }

    public AzImportExportLocation Location { get; }

    public async Task<CommandResult> Create(AzImportExportCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzImportExportDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzImportExportListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImportExportListOptions(), token);
    }

    public async Task<CommandResult> Show(AzImportExportShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzImportExportUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}