using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "restore")]
public class AzBackupRestoreFiles
{
    public AzBackupRestoreFiles(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> MountRp(AzBackupRestoreFilesMountRpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupRestoreFilesMountRpOptions(), token);
    }

    public async Task<CommandResult> UnmountRp(AzBackupRestoreFilesUnmountRpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupRestoreFilesUnmountRpOptions(), token);
    }
}