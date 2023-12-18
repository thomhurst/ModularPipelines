using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage")]
public class AzStorageDirectory
{
    public AzStorageDirectory(
        AzStorageDirectoryCreate create,
        AzStorageDirectoryDelete delete,
        AzStorageDirectoryExists exists,
        AzStorageDirectoryList list,
        AzStorageDirectoryMetadata metadata,
        AzStorageDirectoryShow show,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        ExistsCommands = exists;
        ListCommands = list;
        Metadata = metadata;
        ShowCommands = show;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageDirectoryCreate CreateCommands { get; }

    public AzStorageDirectoryDelete DeleteCommands { get; }

    public AzStorageDirectoryExists ExistsCommands { get; }

    public AzStorageDirectoryList ListCommands { get; }

    public AzStorageDirectoryMetadata Metadata { get; }

    public AzStorageDirectoryShow ShowCommands { get; }

    public async Task<CommandResult> Create(AzStorageDirectoryCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStorageDirectoryDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzStorageDirectoryExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStorageDirectoryListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStorageDirectoryShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}