using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzStorage
{
    public AzStorage(
        AzStorageAccount account,
        AzStorageAzcopy azcopy,
        AzStorageBlob blob,
        AzStorageContainer container,
        AzStorageContainerRm containerRm,
        AzStorageCors cors,
        AzStorageDirectory directory,
        AzStorageEntity entity,
        AzStorageFile file,
        AzStorageFs fs,
        AzStorageLogging logging,
        AzStorageMessage message,
        AzStorageMetrics metrics,
        AzStorageQueue queue,
        AzStorageShare share,
        AzStorageShareRm shareRm,
        AzStorageTable table,
        ICommand internalCommand
    )
    {
        Account = account;
        Azcopy = azcopy;
        Blob = blob;
        Container = container;
        ContainerRm = containerRm;
        Cors = cors;
        Directory = directory;
        Entity = entity;
        File = file;
        Fs = fs;
        Logging = logging;
        Message = message;
        Metrics = metrics;
        Queue = queue;
        Share = share;
        ShareRm = shareRm;
        Table = table;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageAccount Account { get; }

    public AzStorageAzcopy Azcopy { get; }

    public AzStorageBlob Blob { get; }

    public AzStorageContainer Container { get; }

    public AzStorageContainerRm ContainerRm { get; }

    public AzStorageCors Cors { get; }

    public AzStorageDirectory Directory { get; }

    public AzStorageEntity Entity { get; }

    public AzStorageFile File { get; }

    public AzStorageFs Fs { get; }

    public AzStorageLogging Logging { get; }

    public AzStorageMessage Message { get; }

    public AzStorageMetrics Metrics { get; }

    public AzStorageQueue Queue { get; }

    public AzStorageShare Share { get; }

    public AzStorageShareRm ShareRm { get; }

    public AzStorageTable Table { get; }

    public async Task<CommandResult> Copy(AzStorageCopyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageCopyOptions(), token);
    }

    public async Task<CommandResult> Remove(AzStorageRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageRemoveOptions(), token);
    }
}