using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

internal sealed class CliScrapeProvenance
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    private readonly ConcurrentDictionary<string, CliHelpInvocation> _helpInvocations =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Help paths whose latest invocation timed out after every retry. Those commands are
    /// unavailable in this scrape rather than absent from the tool, so coverage validation
    /// must not report them as removals.
    /// </summary>
    public IReadOnlyList<string> TimedOutHelpPaths =>
        _helpInvocations.Values
            .Where(static invocation => invocation.TimedOut)
            .Select(static invocation => invocation.CommandPath)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();

    public void Record(
        IReadOnlyList<string> commandPath,
        string arguments,
        CliCommandResult result,
        bool preserveRawHelp = false)
    {
        var path = string.Join(' ', commandPath);
        _helpInvocations[path] = new CliHelpInvocation
        {
            CommandPath = path,
            Arguments = arguments,
            ExitCode = result.ExitCode,
            StandardOutputLength = result.StandardOutput.Length,
            StandardErrorLength = result.StandardError.Length,
            OutputSha256 = Fingerprint(result.CombinedOutput),
            RawHelp = result.CombinedOutput,
            PreserveRawHelp = preserveRawHelp || commandPath.Count == 1 || result.ExitCode != 0,
            TimedOut = result.TimedOut,
        };
    }

    public void RecordCacheHit(IReadOnlyList<string> commandPath, string helpText)
    {
        var path = string.Join(' ', commandPath);
        _helpInvocations.TryAdd(path, new CliHelpInvocation
        {
            CommandPath = path,
            Arguments = "<cached>",
            ExitCode = 0,
            StandardOutputLength = helpText.Length,
            StandardErrorLength = 0,
            OutputSha256 = Fingerprint(helpText),
            RawHelp = helpText,
            PreserveRawHelp = commandPath.Count == 1,
        });
    }

    public void PreserveGroupHelp(IReadOnlyList<string> commandPath)
    {
        var path = string.Join(' ', commandPath);
        if (_helpInvocations.TryGetValue(path, out var invocation))
        {
            _helpInvocations[path] = invocation with
            {
                PreserveRawHelp = true,
            };
        }
    }

    public void DiscardLeafHelp(IReadOnlyList<string> commandPath)
    {
        var path = string.Join(' ', commandPath);
        if (_helpInvocations.TryGetValue(path, out var invocation)
            && !invocation.PreserveRawHelp)
        {
            _helpInvocations[path] = invocation with { RawHelp = null };
        }
    }

    public async Task<string?> WriteCoverageFailureDiagnosticsAsync(
        string outputDirectory,
        CommandCoverageEvaluation coverage,
        CancellationToken cancellationToken)
    {
        var toolName = coverage.Manifest.ToolName;
        var timedOutHelpPaths = coverage.TimedOutCommands;
        var requestedHelpPaths = coverage.RemovedCommands
            .SelectMany(GetAncestorCommands)
            .Concat(timedOutHelpPaths)
            .Append(toolName)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var invocations = requestedHelpPaths
            .Select(path => _helpInvocations.GetValueOrDefault(path))
            .Where(static invocation => invocation is not null)
            .Select(static invocation => invocation!)
            .ToArray();
        var missingHelpPaths = requestedHelpPaths
            .Where(path => !_helpInvocations.ContainsKey(path))
            .ToArray();
        var diagnostics = new CliCoverageFailureDiagnostics
        {
            ToolName = toolName,
            BaselineToolVersion = coverage.PreviousToolVersion,
            CurrentToolVersion = coverage.Manifest.ToolVersion,
            BaselineCommandCount = coverage.PreviousCommandCount,
            CurrentCommandCount = coverage.Manifest.CommandCount,
            RemovedCommands = coverage.RemovedCommands,
            RequestedHelpPaths = requestedHelpPaths,
            MissingHelpPaths = missingHelpPaths,
            TimedOutHelpPaths = timedOutHelpPaths,
            HelpInvocations = invocations,
        };

        var diagnosticsPath = Path.Combine(
            outputDirectory,
            "artifacts",
            "options-generator-diagnostics",
            GetSafeToolDirectoryName(toolName),
            "command-coverage-failure.json");
        Directory.CreateDirectory(Path.GetDirectoryName(diagnosticsPath)!);
        await File.WriteAllTextAsync(
            diagnosticsPath,
            JsonSerializer.Serialize(diagnostics, JsonOptions) + Environment.NewLine,
            cancellationToken);
        return diagnosticsPath;
    }

    private static IEnumerable<string> GetAncestorCommands(string command)
    {
        var separator = command.LastIndexOf(' ');
        while (separator > 0)
        {
            command = command[..separator];
            yield return command;
            separator = command.LastIndexOf(' ');
        }
    }

    private static string GetSafeToolDirectoryName(string toolName)
    {
        var safeName = new string(toolName
            .Select(character => char.IsAsciiLetterOrDigit(character) || character is '-' or '_'
                ? character
                : '_')
            .ToArray());
        return safeName.Length > 0 ? safeName : "unknown-tool";
    }

    private static string Fingerprint(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
}

internal sealed record CliHelpInvocation
{
    public required string CommandPath { get; init; }

    public required string Arguments { get; init; }

    public required int ExitCode { get; init; }

    public required int StandardOutputLength { get; init; }

    public required int StandardErrorLength { get; init; }

    public required string OutputSha256 { get; init; }

    public string? RawHelp { get; init; }

    internal bool PreserveRawHelp { get; init; }

    /// <summary>
    /// Whether this invocation was abandoned by the executor's timeout, so the command is
    /// unavailable in this scrape rather than absent from the tool.
    /// </summary>
    internal bool TimedOut { get; init; }
}

internal sealed record CliCoverageFailureDiagnostics
{
    public required string ToolName { get; init; }

    public string? BaselineToolVersion { get; init; }

    public string? CurrentToolVersion { get; init; }

    public int? BaselineCommandCount { get; init; }

    public required int CurrentCommandCount { get; init; }

    public required IReadOnlyList<string> RemovedCommands { get; init; }

    public required IReadOnlyList<string> RequestedHelpPaths { get; init; }

    public required IReadOnlyList<string> MissingHelpPaths { get; init; }

    public required IReadOnlyList<string> TimedOutHelpPaths { get; init; }

    public required IReadOnlyList<CliHelpInvocation> HelpInvocations { get; init; }
}
