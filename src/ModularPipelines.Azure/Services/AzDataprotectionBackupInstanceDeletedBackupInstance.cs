using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance")]
public class AzDataprotectionBackupInstanceDeletedBackupInstance
{
    public AzDataprotectionBackupInstanceDeletedBackupInstance(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzDataprotectionBackupInstanceDeletedBackupInstanceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDataprotectionBackupInstanceDeletedBackupInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupInstanceDeletedBackupInstanceShowOptions(), token);
    }

    public async Task<CommandResult> Undelete(AzDataprotectionBackupInstanceDeletedBackupInstanceUndeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupInstanceDeletedBackupInstanceUndeleteOptions(), token);
    }
}