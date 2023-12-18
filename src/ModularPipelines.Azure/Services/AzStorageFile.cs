using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage")]
public class AzStorageFile
{
    public AzStorageFile(
        AzStorageFileCopy copy,
        AzStorageFileDelete delete,
        AzStorageFileDeleteBatch deleteBatch,
        AzStorageFileDownload download,
        AzStorageFileDownloadBatch downloadBatch,
        AzStorageFileExists exists,
        AzStorageFileGenerateSas generateSas,
        AzStorageFileList list,
        AzStorageFileMetadata metadata,
        AzStorageFileResize resize,
        AzStorageFileShow show,
        AzStorageFileUpdate update,
        AzStorageFileUpload upload,
        AzStorageFileUploadBatch uploadBatch,
        AzStorageFileUrl url,
        ICommand internalCommand
    )
    {
        Copy = copy;
        DeleteCommands = delete;
        DeleteBatchCommands = deleteBatch;
        DownloadCommands = download;
        DownloadBatchCommands = downloadBatch;
        ExistsCommands = exists;
        GenerateSasCommands = generateSas;
        ListCommands = list;
        Metadata = metadata;
        ResizeCommands = resize;
        ShowCommands = show;
        UpdateCommands = update;
        UploadCommands = upload;
        UploadBatchCommands = uploadBatch;
        UrlCommands = url;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageFileCopy Copy { get; }

    public AzStorageFileDelete DeleteCommands { get; }

    public AzStorageFileDeleteBatch DeleteBatchCommands { get; }

    public AzStorageFileDownload DownloadCommands { get; }

    public AzStorageFileDownloadBatch DownloadBatchCommands { get; }

    public AzStorageFileExists ExistsCommands { get; }

    public AzStorageFileGenerateSas GenerateSasCommands { get; }

    public AzStorageFileList ListCommands { get; }

    public AzStorageFileMetadata Metadata { get; }

    public AzStorageFileResize ResizeCommands { get; }

    public AzStorageFileShow ShowCommands { get; }

    public AzStorageFileUpdate UpdateCommands { get; }

    public AzStorageFileUpload UploadCommands { get; }

    public AzStorageFileUploadBatch UploadBatchCommands { get; }

    public AzStorageFileUrl UrlCommands { get; }

    public async Task<CommandResult> Delete(AzStorageFileDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBatch(AzStorageFileDeleteBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Download(AzStorageFileDownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DownloadBatch(AzStorageFileDownloadBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzStorageFileExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateSas(AzStorageFileGenerateSasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStorageFileListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Resize(AzStorageFileResizeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStorageFileShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzStorageFileUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Upload(AzStorageFileUploadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UploadBatch(AzStorageFileUploadBatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Url(AzStorageFileUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

