using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup")]
public class AzBackupRestore
{
    public AzBackupRestore(
        AzBackupRestoreFiles files,
        ICommand internalCommand
    )
    {
        Files = files;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBackupRestoreFiles Files { get; }

    public async Task<CommandResult> RestoreAzurefiles(AzBackupRestoreRestoreAzurefilesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreAzurefileshare(AzBackupRestoreRestoreAzurefileshareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreAzurewl(AzBackupRestoreRestoreAzurewlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreDisks(AzBackupRestoreRestoreDisksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}