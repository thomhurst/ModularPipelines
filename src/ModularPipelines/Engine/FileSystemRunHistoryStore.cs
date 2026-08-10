using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal sealed class FileSystemRunHistoryStore(
    IOptions<PipelineOptions> pipelineOptions,
    ILogger<FileSystemRunHistoryStore> logger,
    RunReportPathResolver? pathResolver = null) : IRunHistoryStore
{
    private const string OwnedFilePrefix = "modularpipelines-run-";
    private const string FileTimestampFormat = "yyyyMMddHHmmssfffffff";
    private const int MinimumCompatibleSchemaVersion = 1;
    private readonly RunReportPathResolver _pathResolver = pathResolver ?? new RunReportPathResolver();
    private static readonly TimeSpan StaleTemporaryFileAge = TimeSpan.FromDays(1);

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

        var incompatibleSchemaLogged = false;
        var files = GetHistoryFilesNewestFirst(
            directory,
            $"{GetPipelineFilePrefix(pipelineIdentity)}*.json");
        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await using var stream = new FileStream(
                    file,
                    new FileStreamOptions
                    {
                        Access = FileAccess.Read,
                        Mode = FileMode.Open,
                        Options = FileOptions.Asynchronous | FileOptions.SequentialScan,
                        Share = FileShare.Read | FileShare.Delete,
                    });
                var (report, incompatibleSchemaVersion) = await ReadReportAsync(stream, cancellationToken)
                    .ConfigureAwait(false);
                if (incompatibleSchemaVersion is { } schemaVersion)
                {
                    if (!incompatibleSchemaLogged)
                    {
                        logger.LogWarning(
                            "Skipped pipeline run history with schema version {SchemaVersion}; supported versions are {MinimumSchemaVersion} through {CurrentSchemaVersion}",
                            schemaVersion,
                            MinimumCompatibleSchemaVersion,
                            PipelineRunReport.CurrentSchemaVersion);
                        incompatibleSchemaLogged = true;
                    }

                    continue;
                }

                if (report is not null
                    && string.Equals(report.PipelineIdentity, pipelineIdentity, StringComparison.Ordinal))
                {
                    return report;
                }
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Text.Json.JsonException)
            {
                logger.LogWarning(exception, "Could not read pipeline run history file {HistoryFile}", file);
            }
        }

        return null;
    }

    private static async Task<(PipelineRunReport? Report, int? IncompatibleSchemaVersion)> ReadReportAsync(
        Stream stream,
        CancellationToken cancellationToken)
    {
        using var document = await JsonDocument.ParseAsync(
                stream,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        var schemaVersion = ReadSchemaVersion(document.RootElement);
        if (!IsSchemaVersionCompatible(schemaVersion))
        {
            return (null, schemaVersion);
        }

        return (
            document.RootElement.Deserialize(RunReportJsonContext.Default.PipelineRunReport),
            null);
    }

    private static int ReadSchemaVersion(JsonElement root)
    {
        if (root.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException("Pipeline run history must contain a JSON object.");
        }

        if (!root.TryGetProperty("schemaVersion", out var schemaVersionElement))
        {
            return PipelineRunReport.CurrentSchemaVersion;
        }

        if (schemaVersionElement.ValueKind != JsonValueKind.Number
            || !schemaVersionElement.TryGetInt32(out var schemaVersion))
        {
            throw new JsonException("Pipeline run history schemaVersion must be a 32-bit integer.");
        }

        return schemaVersion;
    }

    internal static bool IsSchemaVersionCompatible(
        int schemaVersion,
        int currentSchemaVersion = PipelineRunReport.CurrentSchemaVersion) =>
        schemaVersion >= MinimumCompatibleSchemaVersion && schemaVersion <= currentSchemaVersion;

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
        var runId = Guid.TryParse(report.RunId, out var parsedRunId)
            ? parsedRunId.ToString("N")
            : Guid.NewGuid().ToString("N");
        var fileName = $"{filePrefix}{report.End.UtcDateTime:yyyyMMddHHmmssfffffff}-{runId}.json";
        var path = Path.Combine(directory, fileName);
        await AtomicFileWriter.WriteAllTextAsync(
                path,
                RunReportJsonSerializer.Serialize(report),
                cancellationToken)
            .ConfigureAwait(false);

        PruneStaleTemporaryFiles(directory, cancellationToken);
        PruneFiles(directory, $"{filePrefix}*.json", retention, cancellationToken);
        PruneFiles(
            directory,
            $"{OwnedFilePrefix}*.json",
            pipelineOptions.Value.RunReport.GlobalHistoryRetention,
            cancellationToken);
    }

    private void PruneFiles(
        string directory,
        string searchPattern,
        int retention,
        CancellationToken cancellationToken)
    {
        if (retention <= 0)
        {
            return;
        }
        var staleFiles = GetHistoryFilesNewestFirst(directory, searchPattern)
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

    private static IEnumerable<string> GetHistoryFilesNewestFirst(
        string directory,
        string searchPattern) =>
        Directory
            .EnumerateFiles(directory, searchPattern, SearchOption.TopDirectoryOnly)
            .OrderByDescending(GetHistoryTimestamp)
            .ThenByDescending(static file => Path.GetFileName(file), StringComparer.Ordinal);

    private static DateTime GetHistoryTimestamp(string path)
    {
        var fileName = Path.GetFileName(path);
        var uniqueIdSeparator = fileName.LastIndexOf('-');
        var timestampSeparator = uniqueIdSeparator > 0
            ? fileName.LastIndexOf('-', uniqueIdSeparator - 1)
            : -1;
        var timestampStart = timestampSeparator + 1;
        var timestampLength = uniqueIdSeparator - timestampStart;
        if (timestampLength != FileTimestampFormat.Length
            || !DateTime.TryParseExact(
                fileName.AsSpan(timestampStart, timestampLength),
                FileTimestampFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var timestamp))
        {
            return DateTime.MinValue;
        }

        return timestamp;
    }

    private void PruneStaleTemporaryFiles(string directory, CancellationToken cancellationToken)
    {
        var staleBefore = DateTime.UtcNow - StaleTemporaryFileAge;
        foreach (var temporaryFile in Directory.EnumerateFiles(
                     directory,
                     AtomicFileWriter.TemporaryFilePattern,
                     SearchOption.TopDirectoryOnly))
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                if (File.GetLastWriteTimeUtc(temporaryFile) <= staleBefore)
                {
                    File.Delete(temporaryFile);
                }
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                logger.LogWarning(exception, "Could not prune temporary run history file {HistoryFile}", temporaryFile);
            }
        }
    }

    private string GetHistoryDirectory() =>
        _pathResolver.Resolve(pipelineOptions.Value.RunReport.HistoryDirectory);

    private static string GetPipelineFilePrefix(string? pipelineIdentity)
    {
        var identityBytes = Encoding.UTF8.GetBytes(pipelineIdentity ?? string.Empty);
        var identityHash = Convert.ToHexString(SHA256.HashData(identityBytes)).ToLowerInvariant();
        return $"{OwnedFilePrefix}{identityHash}-";
    }
}
