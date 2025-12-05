using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage")]
public class AzStorageFs
{
    public AzStorageFs(
        AzStorageFsAccess access,
        AzStorageFsDirectory directory,
        AzStorageFsFile file,
        AzStorageFsMetadata metadata,
        AzStorageFsServiceProperties serviceProperties,
        ICommand internalCommand
    )
    {
        Access = access;
        Directory = directory;
        File = file;
        Metadata = metadata;
        ServiceProperties = serviceProperties;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageFsAccess Access { get; }

    public AzStorageFsDirectory Directory { get; }

    public AzStorageFsFile File { get; }

    public AzStorageFsMetadata Metadata { get; }

    public AzStorageFsServiceProperties ServiceProperties { get; }

    public async Task<CommandResult> Create(AzStorageFsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStorageFsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzStorageFsExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateSas(AzStorageFsGenerateSasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStorageFsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageFsListOptions(), token);
    }

    public async Task<CommandResult> ListDeletedPath(AzStorageFsListDeletedPathOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStorageFsShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UndeletePath(AzStorageFsUndeletePathOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}