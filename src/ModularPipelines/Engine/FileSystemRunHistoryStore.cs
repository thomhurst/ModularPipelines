using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal sealed class FileSystemRunHistoryStore(
    IOptions<PipelineOptions> pipelineOptions,
    ILogger<FileSystemRunHistoryStore> logger) : IRunHistoryStore
{
    private const string OwnedFilePrefix = "modularpipelines-run-";

    public Task<PipelineRunReport?> GetLatestAsync(
        string pipelineIdentity,
        CancellationToken cancellationToken = default) =>
        GetLatestAsyncCore(pipelineIdentity, cancellationToken);

    private async Task<PipelineRunReport?> GetLatestAsyncCore(
        string pipelineIdentity,
        CancellationToken cancellationToken)
    {
        var directory = GetHistoryDirectory();
        if (!Directory.Exists(directory))
        {
            return null;
        }

        PipelineRunReport? latestReport = null;
        foreach (var file in Directory.EnumerateFiles(
                     directory,
                     $"{GetPipelineFilePrefix(pipelineIdentity)}*.json",
                     SearchOption.TopDirectoryOnly))
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var json = await File.ReadAllTextAsync(file, cancellationToken).ConfigureAwait(false);
                if (RunReportJsonSerializer.Deserialize(json) is
                    { SchemaVersion: PipelineRunReport.CurrentSchemaVersion } report
                    && string.Equals(
                        report.PipelineIdentity,
                        pipelineIdentity,
                        StringComparison.Ordinal))
                {
                    if (latestReport is null || report.End > latestReport.End)
                    {
                        latestReport = report;
                    }
                }
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Text.Json.JsonException)
            {
                logger.LogWarning(exception, "Could not read pipeline run history file {HistoryFile}", file);
            }
        }

        return latestReport;
    }

    public async Task SaveAsync(
        PipelineRunReport report,
        CancellationToken cancellationToken = default)
    {
        var retention = pipelineOptions.Value.RunReport.HistoryRetention;
        if (retention <= 0)
        {
            return;
        }

        var directory = GetHistoryDirectory();
        Directory.CreateDirectory(directory);
        var filePrefix = GetPipelineFilePrefix(report.PipelineIdentity);
        var fileName = $"{filePrefix}{report.End.UtcDateTime:yyyyMMddHHmmssfffffff}-{Guid.NewGuid():N}.json";
        var path = Path.Combine(directory, fileName);
        await File.WriteAllTextAsync(
                path,
                RunReportJsonSerializer.Serialize(report),
                cancellationToken)
            .ConfigureAwait(false);

        var staleFiles = Directory
            .EnumerateFiles(directory, $"{filePrefix}*.json", SearchOption.TopDirectoryOnly)
            .OrderByDescending(static file => Path.GetFileName(file), StringComparer.Ordinal)
            .Skip(retention)
            .ToArray();
        foreach (var staleFile in staleFiles)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                File.Delete(staleFile);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                logger.LogWarning(exception, "Could not prune pipeline run history file {HistoryFile}", staleFile);
            }
        }
    }

    private string GetHistoryDirectory() =>
        Path.GetFullPath(pipelineOptions.Value.RunReport.HistoryDirectory);

    private static string GetPipelineFilePrefix(string? pipelineIdentity)
    {
        var identityBytes = Encoding.UTF8.GetBytes(pipelineIdentity ?? string.Empty);
        var identityHash = Convert.ToHexString(SHA256.HashData(identityBytes)).ToLowerInvariant();
        return $"{OwnedFilePrefix}{identityHash}-";
    }
}
