using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account")]
public class AzStorageAccountMigration
{
    public AzStorageAccountMigration(
        AzStorageAccountMigrationShow show,
        AzStorageAccountMigrationStart start,
        ICommand internalCommand
    )
    {
        ShowCommands = show;
        StartCommands = start;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageAccountMigrationShow ShowCommands { get; }

    public AzStorageAccountMigrationStart StartCommands { get; }

    public async Task<CommandResult> Show(AzStorageAccountMigrationShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzStorageAccountMigrationStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}