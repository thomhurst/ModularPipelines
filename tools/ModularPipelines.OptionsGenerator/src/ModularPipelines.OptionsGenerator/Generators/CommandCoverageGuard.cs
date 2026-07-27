using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

internal static partial class CommandCoverageGuard
{
    private const int ManifestFormatVersion = 1;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static CommandCoverageEvaluation Evaluate(
        CliToolDefinition tool,
        string outputDirectory,
        bool approveShrinkage)
    {
        var commands = NormalizeCommands(tool.Commands.Select(command => command.FullCommand));
        var manifestPath = GetManifestPath(tool, outputDirectory);
        var (previous, usedGeneratedApiBaseline) = ReadBaseline(tool, outputDirectory, manifestPath);
        var exclusions = ValidateExclusions(tool.CommandCoverage.Exclusions);
        var excludedCommands = exclusions
            .Select(exclusion => NormalizeCommand(exclusion.Command))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var addedCommands = previous is null
            ? []
            : commands.Except(previous.Commands, StringComparer.OrdinalIgnoreCase).ToArray();
        var removedCommands = previous is null
            ? []
            : previous.Commands.Except(commands, StringComparer.OrdinalIgnoreCase).ToArray();
        var unapprovedRemovedCommands = removedCommands
            .Where(command => !excludedCommands.Contains(command))
            .ToArray();
        var knownGroupsWithoutChildren = GetKnownGroupsWithoutChildren(
            previous,
            commands,
            excludedCommands);
        var missingSentinels = GetMissingSentinels(tool.CommandCoverage, commands, excludedCommands);
        var violations = GetViolations(
            tool.CommandCoverage,
            commands.Count,
            missingSentinels,
            unapprovedRemovedCommands,
            knownGroupsWithoutChildren,
            approveShrinkage);

        var manifest = CreateManifest(tool.ToolName, tool.ToolVersion, commands, exclusions);

        return new CommandCoverageEvaluation
        {
            ManifestPath = manifestPath,
            Manifest = manifest,
            HasPreviousBaseline = previous is not null,
            UsedGeneratedApiBaseline = usedGeneratedApiBaseline,
            AddedCommands = addedCommands,
            RemovedCommands = removedCommands,
            KnownGroupsWithoutChildren = knownGroupsWithoutChildren,
            Violations = violations,
            ShrinkageApproved = approveShrinkage,
        };
    }

    private static (CommandCoverageManifest? Manifest, bool UsedGeneratedApiBaseline) ReadBaseline(
        CliToolDefinition tool,
        string outputDirectory,
        string manifestPath)
    {
        var manifest = ReadManifest(manifestPath);
        if (manifest is not null)
        {
            return (manifest, false);
        }

        var generatedApiBaseline = ReadGeneratedApiBaseline(tool, outputDirectory);
        return (generatedApiBaseline, generatedApiBaseline is not null);
    }

    private static IReadOnlyList<string> GetKnownGroupsWithoutChildren(
        CommandCoverageManifest? previous,
        IReadOnlyList<string> commands,
        IReadOnlySet<string> excludedCommands) =>
        previous?.CommandGroups
            .Where(group => HasChildren(group, previous.Commands) && !HasChildren(group, commands))
            .Where(group => !previous.Commands
                .Where(command => IsChildOf(group, command))
                .All(excludedCommands.Contains))
            .ToArray()
        ?? [];

    private static IReadOnlyList<string> GetMissingSentinels(
        CliCommandCoveragePolicy policy,
        IReadOnlyList<string> commands,
        IReadOnlySet<string> excludedCommands) =>
        policy.SentinelCommands
            .Select(NormalizeCommand)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Where(sentinel => !commands.Contains(sentinel, StringComparer.OrdinalIgnoreCase))
            .Where(sentinel => !excludedCommands.Contains(sentinel))
            .ToArray();

    private static IReadOnlyList<string> GetViolations(
        CliCommandCoveragePolicy policy,
        int commandCount,
        IReadOnlyList<string> missingSentinels,
        IReadOnlyList<string> removedCommands,
        IReadOnlyList<string> groupsWithoutChildren,
        bool approveShrinkage)
    {
        var violations = new List<string>();

        if (policy.MinimumCommandCount is < 1)
        {
            violations.Add("MinimumCommandCount must be greater than zero when configured.");
        }
        else if (policy.MinimumCommandCount is { } minimum && commandCount < minimum)
        {
            violations.Add($"Command count {commandCount} is below the configured minimum of {minimum}.");
        }

        AddViolation(violations, "Missing sentinel commands", missingSentinels);
        if (!approveShrinkage)
        {
            AddViolation(violations, "Removed commands require explicit approval", removedCommands);
            AddViolation(violations, "Known command groups lost all children", groupsWithoutChildren);
        }

        return violations;
    }

    private static void AddViolation(
        ICollection<string> violations,
        string message,
        IReadOnlyCollection<string> values)
    {
        if (values.Count > 0)
        {
            violations.Add($"{message}: {string.Join(", ", values)}.");
        }
    }

    public static async Task WriteManifestAsync(
        CommandCoverageEvaluation evaluation,
        CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(evaluation.ManifestPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(evaluation.Manifest, JsonOptions) + Environment.NewLine;
        await File.WriteAllTextAsync(evaluation.ManifestPath, json, cancellationToken);
    }

    private static CommandCoverageManifest? ReadManifest(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        var manifest = JsonSerializer.Deserialize<CommandCoverageManifest>(
                           File.ReadAllText(path),
                           JsonOptions)
                       ?? throw new InvalidOperationException($"Command coverage manifest is empty: {path}");
        if (manifest.FormatVersion != ManifestFormatVersion)
        {
            throw new InvalidOperationException(
                $"Unsupported command coverage manifest format {manifest.FormatVersion} in {path}.");
        }

        var commands = NormalizeCommands(manifest.Commands);
        var commandGroups = GetCommandGroups(commands);
        if (manifest.CommandCount != commands.Count
            || !string.Equals(manifest.CommandTreeSha256, Fingerprint(commands), StringComparison.OrdinalIgnoreCase)
            || !NormalizeCommands(manifest.CommandGroups).SequenceEqual(commandGroups, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Command coverage manifest metadata does not match its command tree: {path}. "
                + "If the normalization, grouping, or fingerprint algorithm changed intentionally, "
                + "bump ManifestFormatVersion and regenerate the manifests.");
        }

        return manifest with
        {
            Commands = commands,
            CommandGroups = commandGroups,
        };
    }

    private static CommandCoverageManifest? ReadGeneratedApiBaseline(
        CliToolDefinition tool,
        string outputDirectory)
    {
        var optionsDirectory = Path.Combine(outputDirectory, tool.OutputDirectory, "Options");
        if (!Directory.Exists(optionsDirectory))
        {
            return null;
        }

        var commands = NormalizeCommands(
            Directory.EnumerateFiles(
                    optionsDirectory,
                    $"{tool.NamespacePrefix}*Options.Generated.cs",
                    SearchOption.TopDirectoryOnly)
                .SelectMany(ReadGeneratedSubcommands)
                .Select(parts => string.Join(' ', parts.Prepend(tool.ToolName))));

        return commands.Count == 0
            ? null
            : CreateManifest(tool.ToolName, null, commands, []);
    }

    private static IEnumerable<IReadOnlyList<string>> ReadGeneratedSubcommands(string path)
    {
        var content = File.ReadAllText(path);
        foreach (Match attribute in CliSubCommandAttributePattern().Matches(content))
        {
            var commandParts = QuotedStringPattern()
                .Matches(attribute.Groups["arguments"].Value)
                .Select(match => Regex.Unescape(match.Groups["value"].Value))
                .ToArray();
            if (commandParts.Length > 0)
            {
                yield return commandParts;
            }
        }
    }

    private static CommandCoverageManifest CreateManifest(
        string toolName,
        string? toolVersion,
        IReadOnlyList<string> commands,
        IReadOnlyList<CliCommandCoverageExclusion> exclusions) =>
        new()
        {
            FormatVersion = ManifestFormatVersion,
            ToolName = toolName,
            ToolVersion = toolVersion,
            CommandCount = commands.Count,
            CommandTreeSha256 = Fingerprint(commands),
            Commands = commands,
            CommandGroups = GetCommandGroups(commands),
            Exclusions = exclusions,
        };

    private static IReadOnlyList<CliCommandCoverageExclusion> ValidateExclusions(
        IReadOnlyList<CliCommandCoverageExclusion> exclusions)
    {
        foreach (var exclusion in exclusions)
        {
            if (string.IsNullOrWhiteSpace(exclusion.Command) || string.IsNullOrWhiteSpace(exclusion.Reason))
            {
                throw new InvalidOperationException(
                    "Every command coverage exclusion requires a full command and a non-empty reason.");
            }
        }

        var duplicate = exclusions
            .GroupBy(exclusion => NormalizeCommand(exclusion.Command), StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
        {
            throw new InvalidOperationException($"Duplicate command coverage exclusion: {duplicate.Key}");
        }

        return exclusions
            .Select(exclusion => exclusion with { Command = NormalizeCommand(exclusion.Command) })
            .OrderBy(exclusion => exclusion.Command, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static IReadOnlyList<string> NormalizeCommands(IEnumerable<string> commands) =>
        commands
            .Select(NormalizeCommand)
            .Where(command => command.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(command => command, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private static string NormalizeCommand(string command) =>
        string.Join(' ', command.Split((char[]?) null, StringSplitOptions.RemoveEmptyEntries));

    private static IReadOnlyList<string> GetCommandGroups(IReadOnlyList<string> commands) =>
        commands
            .SelectMany(command =>
            {
                var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return Enumerable.Range(2, Math.Max(0, parts.Length - 2))
                    .Select(length => string.Join(' ', parts.Take(length)));
            })
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(group => group, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private static bool HasChildren(string group, IReadOnlyList<string> commands) =>
        commands.Any(command => IsChildOf(group, command));

    private static bool IsChildOf(string group, string command) =>
        command.Length > group.Length
        && command.StartsWith(group, StringComparison.OrdinalIgnoreCase)
        && command[group.Length] == ' ';

    private static string Fingerprint(IReadOnlyList<string> commands) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(string.Join('\n', commands))))
            .ToLowerInvariant();

    private static string GetManifestPath(CliToolDefinition tool, string outputDirectory) =>
        Path.Combine(
            outputDirectory,
            tool.OutputDirectory,
            "Generated",
            $"{tool.NamespacePrefix}.CommandCoverage.json");

    [GeneratedRegex(@"\[CliSubCommand\((?<arguments>[^\)]*)\)\]")]
    private static partial Regex CliSubCommandAttributePattern();

    [GeneratedRegex(@"""(?<value>[^""]+)""")]
    private static partial Regex QuotedStringPattern();
}

internal sealed record CommandCoverageManifest
{
    public required int FormatVersion { get; init; }

    public required string ToolName { get; init; }

    public string? ToolVersion { get; init; }

    public required int CommandCount { get; init; }

    public required string CommandTreeSha256 { get; init; }

    public required IReadOnlyList<string> Commands { get; init; }

    public required IReadOnlyList<string> CommandGroups { get; init; }

    public required IReadOnlyList<CliCommandCoverageExclusion> Exclusions { get; init; }
}

internal sealed record CommandCoverageEvaluation
{
    public required string ManifestPath { get; init; }

    public required CommandCoverageManifest Manifest { get; init; }

    public required bool HasPreviousBaseline { get; init; }

    public bool UsedGeneratedApiBaseline { get; init; }

    public required IReadOnlyList<string> AddedCommands { get; init; }

    public required IReadOnlyList<string> RemovedCommands { get; init; }

    public required IReadOnlyList<string> KnownGroupsWithoutChildren { get; init; }

    public required IReadOnlyList<string> Violations { get; init; }

    public required bool ShrinkageApproved { get; init; }
}
