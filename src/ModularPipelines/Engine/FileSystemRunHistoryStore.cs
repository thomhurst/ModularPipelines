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
    ILogger<FileSystemRunHistoryStore> logger) : IRunHistoryStore
{
    private const string OwnedFilePrefix = "modularpipelines-run-";
    private const int MinimumCompatibleSchemaVersion = 1;

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
        var incompatibleSchemaLogged = false;
        foreach (var file in Directory.EnumerateFiles(
                     directory,
                     $"{GetPipelineFilePrefix(pipelineIdentity)}*.json",
                     SearchOption.TopDirectoryOnly))
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var json = await File.ReadAllTextAsync(file, cancellationToken).ConfigureAwait(false);
                if (!TryDeserializeCompatibleReport(json, ref incompatibleSchemaLogged, out var report))
                {
                    continue;
                }

                if (string.Equals(
                        report.PipelineIdentity,
                        pipelineIdentity,
                        StringComparison.Ordinal)
                    && (latestReport is null || report.End > latestReport.End))
                {
                    latestReport = report;
                }
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Text.Json.JsonException)
            {
                logger.LogWarning(exception, "Could not read pipeline run history file {HistoryFile}", file);
            }
        }

        return latestReport;
    }

    private bool TryDeserializeCompatibleReport(
        string json,
        ref bool incompatibleSchemaLogged,
        out PipelineRunReport report)
    {
        using var document = JsonDocument.Parse(json);
        var schemaVersion = ReadSchemaVersion(document.RootElement);
        if (!IsSchemaVersionCompatible(schemaVersion))
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

            report = null!;
            return false;
        }

        var deserializedReport = RunReportJsonSerializer.Deserialize(json);
        if (deserializedReport is null)
        {
            report = null!;
            return false;
        }

        report = deserializedReport;
        return true;
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
        var fileName = $"{filePrefix}{report.End.UtcDateTime:yyyyMMddHHmmssfffffff}-{Guid.NewGuid():N}.json";
        var path = Path.Combine(directory, fileName);
        await AtomicFileWriter.WriteAllTextAsync(
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
