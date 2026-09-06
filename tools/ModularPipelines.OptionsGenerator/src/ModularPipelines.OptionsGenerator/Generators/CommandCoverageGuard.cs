using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.OptionsGenerator.External;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

internal static class CommandCoverageGuard
{
    private const int ManifestFormatVersion = 1;
    private const int MaximumBlanketApprovedRemovals = 5;
    private const int RepresentativeRemovalLimit = 10;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <param name="unavailableHelpPaths">
    /// Command paths whose help invocation never produced a real response (timed out after every
    /// retry, or rejected by the circuit breaker). They and every baseline command beneath them
    /// are unavailable in this scrape rather than removed from the tool, so they always fail
    /// validation and are never covered by <paramref name="approveShrinkage"/>.
    /// </param>
    public static CommandCoverageEvaluation Evaluate(
        CliToolDefinition tool,
        string outputDirectory,
        bool approveShrinkage,
        string? fallbackManifestPath = null,
        StringComparer? pathComparer = null,
        bool allowMissingManifest = false,
        IReadOnlyList<string>? unavailableHelpPaths = null)
    {
        var commands = GetCoverageCommands(tool);
        var manifestPath = GetManifestPath(tool, outputDirectory);
        var previous = ReadBaseline(
            tool,
            outputDirectory,
            manifestPath,
            fallbackManifestPath,
            pathComparer ?? StringComparer.OrdinalIgnoreCase,
            allowMissingManifest);
        var (exclusions, allowedMissingCommands) = ValidateCoveragePolicy(tool.CommandCoverage);
        var unavailableCommands = GetUnavailableCommands(unavailableHelpPaths);
        var (addedCommands, removedCommands) = GetCommandDiff(previous, commands, unavailableCommands);
        var unapprovedRemovedCommands = removedCommands
            .Where(command => !allowedMissingCommands.Contains(command))
            .ToArray();
        var knownGroupsWithoutChildren = GetKnownGroupsWithoutChildren(
            previous,
            commands,
            allowedMissingCommands,
            unavailableCommands);
        var missingSentinels = GetMissingSentinels(tool.CommandCoverage, commands, allowedMissingCommands);
        var hasSameVersionCommandSetDrift = previous is not null
            && HasSameResolvedVersion(previous.ToolVersion, tool.ToolVersion)
            && (addedCommands.Length > 0 || removedCommands.Length > 0);
        var violations = GetViolations(
            tool.CommandCoverage,
            commands.Count,
            missingSentinels,
            addedCommands,
            removedCommands,
            unapprovedRemovedCommands,
            knownGroupsWithoutChildren,
            hasSameVersionCommandSetDrift,
            tool.ToolVersion,
            approveShrinkage,
            previous?.ToolVersion,
            tool.ToolVersion,
            unavailableCommands);

        var manifest = CreateManifest(tool.ToolName, tool.ToolVersion, commands, exclusions);

        return new CommandCoverageEvaluation
        {
            ManifestPath = manifestPath,
            Manifest = manifest,
            HasPreviousBaseline = previous is not null,
            PreviousCommandCount = previous?.CommandCount,
            PreviousToolVersion = previous?.ToolVersion,
            AddedCommands = addedCommands,
            RemovedCommands = removedCommands,
            UnavailableCommands = unavailableCommands,
            KnownGroupsWithoutChildren = knownGroupsWithoutChildren,
            Violations = violations,
            ChangesApproved = approveShrinkage
                              && unapprovedRemovedCommands.Length <= MaximumBlanketApprovedRemovals,
        };
    }

    private static (IReadOnlyList<CliCommandCoverageExclusion> Exclusions, HashSet<string> AllowedMissingCommands)
        ValidateCoveragePolicy(CliCommandCoveragePolicy policy)
    {
        var exclusions = ValidateExclusions(policy.Exclusions);
        var excludedCommands = exclusions
            .Select(exclusion => NormalizeCommand(exclusion.Command))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var conditionallyAvailableCommands = ValidateConditionallyAvailableCommands(
                policy.ConditionallyAvailableCommands)
            .Select(command => NormalizeCommand(command.Command))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var conflictingCommands = excludedCommands
            .Intersect(conditionallyAvailableCommands, StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (conflictingCommands.Length > 0)
        {
            throw new InvalidOperationException(
                "Commands cannot be both excluded and conditionally available: "
                + string.Join(", ", conflictingCommands));
        }

        var allowedMissingCommands = excludedCommands
            .Concat(conditionallyAvailableCommands)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        return (exclusions, allowedMissingCommands);
    }

    /// <summary>
    /// Commands whose help never came back are unavailable, not removed: they (and, for a group
    /// or root path, everything beneath them that the traversal never reached) stay out of the
    /// removal arithmetic and its approval budget and are reported on their own.
    /// </summary>
    private static string[] GetUnavailableCommands(IReadOnlyList<string>? unavailableHelpPaths) =>
        (unavailableHelpPaths ?? [])
            .Select(NormalizeCommand)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private static bool IsUnavailable(string command, IReadOnlyList<string> unavailableCommands) =>
        unavailableCommands.Any(unavailable => IsSameOrChildOf(unavailable, command));

    private static (string[] Added, string[] Removed) GetCommandDiff(
        CommandCoverageManifest? previous,
        IReadOnlyList<string> commands,
        IReadOnlyList<string> unavailableCommands)
    {
        if (previous is null)
        {
            return ([], []);
        }

        var added = commands.Except(previous.Commands, StringComparer.OrdinalIgnoreCase).ToArray();
        var removed = previous.Commands
            .Except(commands, StringComparer.OrdinalIgnoreCase)
            .Where(command => !IsUnavailable(command, unavailableCommands))
            .ToArray();
        return (added, removed);
    }

    private static CommandCoverageManifest? ReadBaseline(
        CliToolDefinition tool,
        string outputDirectory,
        string manifestPath,
        string? fallbackManifestPath,
        StringComparer pathComparer,
        bool allowMissingManifest)
    {
        var manifest = ReadManifest(manifestPath);
        if (manifest is not null)
        {
            return manifest;
        }

        if (!string.IsNullOrEmpty(fallbackManifestPath)
            && !pathComparer.Equals(manifestPath, fallbackManifestPath))
        {
            manifest = ReadManifest(fallbackManifestPath);
            if (manifest is not null)
            {
                return manifest;
            }
        }

        if (!allowMissingManifest && HasGeneratedApi(tool, outputDirectory))
        {
            throw new InvalidOperationException(
                $"Command coverage manifest is missing for '{tool.ToolName}': {manifestPath}. "
                + "Restore the committed manifest before regenerating this tool.");
        }

        return null;
    }

    /// <summary>
    /// Baseline groups that no longer have any child command, unless every missing child is
    /// either excluded by policy or unavailable (its help, or an ancestor's, could not be
    /// scraped), in which case the unavailable-help violation already explains the gap.
    /// </summary>
    private static IReadOnlyList<string> GetKnownGroupsWithoutChildren(
        CommandCoverageManifest? previous,
        IReadOnlyList<string> commands,
        IReadOnlySet<string> excludedCommands,
        IReadOnlyList<string> unavailableCommands) =>
        previous?.CommandGroups
            .Where(group => HasChildren(group, previous.Commands) && !HasChildren(group, commands))
            .Where(group => !previous.Commands
                .Where(command => IsChildOf(group, command))
                .All(command => excludedCommands.Contains(command) || IsUnavailable(command, unavailableCommands)))
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
        IReadOnlyList<string> addedCommands,
        IReadOnlyList<string> removedCommands,
        IReadOnlyList<string> unapprovedRemovedCommands,
        IReadOnlyList<string> groupsWithoutChildren,
        bool hasSameVersionCommandSetDrift,
        string? toolVersion,
        bool approveShrinkage,
        string? previousToolVersion,
        string? currentToolVersion,
        IReadOnlyList<string> unavailableCommands)
    {
        var violations = new List<string>();

        // Never subject to shrinkage approval: the scrape is incomplete, not the tool smaller.
        AddViolation(
            violations,
            "Help was unavailable after all retries (timed out, rejected by the circuit breaker, or the process could not run); rerun the generation instead of approving these as removals",
            unavailableCommands);

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
            if (hasSameVersionCommandSetDrift)
            {
                violations.Add(
                    $"Command set changed while the resolved tool version remained '{toolVersion}'; "
                    + "explicit approval is required. "
                    + FormatCommandDiff("Added", addedCommands)
                    + " "
                    + FormatCommandDiff("Removed", removedCommands));
            }
            else
            {
                AddViolation(
                    violations,
                    "Removed commands require explicit approval",
                    unapprovedRemovedCommands);
            }

            AddViolation(violations, "Known command groups lost all children", groupsWithoutChildren);
        }
        else if (unapprovedRemovedCommands.Count > MaximumBlanketApprovedRemovals)
        {
            var previousVersion = previousToolVersion ?? "unknown version";
            var currentVersion = currentToolVersion ?? "unknown version";
            var representativeRemovals = unapprovedRemovedCommands.Take(RepresentativeRemovalLimit);
            var remainingCount = unapprovedRemovedCommands.Count - RepresentativeRemovalLimit;
            var suffix = remainingCount > 0 ? $" (+{remainingCount} more)" : string.Empty;
            violations.Add(
                $"Blanket approval cannot authorize {unapprovedRemovedCommands.Count} command removals "
                + $"from baseline {previousVersion} to current {currentVersion}; the maximum is "
                + $"{MaximumBlanketApprovedRemovals}. Add explicit command coverage exclusions with "
                + $"reviewed reasons for every additional removal. Representative removals: "
                + $"{string.Join(", ", representativeRemovals)}{suffix}.");
        }

        return violations;
    }

    private static bool HasSameResolvedVersion(string? previousVersion, string? currentVersion) =>
        !string.IsNullOrWhiteSpace(previousVersion)
        && !string.IsNullOrWhiteSpace(currentVersion)
        && string.Equals(previousVersion, currentVersion, StringComparison.OrdinalIgnoreCase);

    private static string FormatCommandDiff(string label, IReadOnlyCollection<string> commands) =>
        commands.Count == 0
            ? $"{label}: none."
            : $"{label}: {string.Join(", ", commands)}.";

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
        CancellationToken cancellationToken,
        string? containmentRoot = null)
    {
        ValidateManifestContainment(evaluation.ManifestPath, containmentRoot);
        var directory = Path.GetDirectoryName(evaluation.ManifestPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        ValidateManifestContainment(evaluation.ManifestPath, containmentRoot);
        var json = JsonSerializer.Serialize(evaluation.Manifest, JsonOptions) + Environment.NewLine;
        await File.WriteAllTextAsync(evaluation.ManifestPath, json, cancellationToken);
    }

    internal static bool IsGeneratedManifest(
        string path,
        string? containmentRoot = null)
    {
        if (!path.EndsWith(
                ".CommandCoverage.json",
                StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        try
        {
            ValidateManifestContainment(path, containmentRoot);
            return ReadManifest(path) is not null;
        }
        catch (JsonException)
        {
            return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    private static void ValidateManifestContainment(
        string manifestPath,
        string? containmentRoot)
    {
        if (containmentRoot is null)
        {
            return;
        }

        ExternalToolDefinitionLoader.ValidateRelativeOutputPath(
            Path.GetRelativePath(containmentRoot, manifestPath),
            containmentRoot,
            "command coverage manifest");
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

    private static bool HasGeneratedApi(
        CliToolDefinition tool,
        string outputDirectory)
    {
        var optionsDirectory = Path.Combine(outputDirectory, tool.OutputDirectory, "Options");
        return Directory.Exists(optionsDirectory)
               && Directory.EnumerateFiles(
                       optionsDirectory,
                       $"{tool.NamespacePrefix}*Options*.cs",
                       SearchOption.TopDirectoryOnly)
                   .Any();
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

    private static IReadOnlyList<CliConditionallyAvailableCommand> ValidateConditionallyAvailableCommands(
        IReadOnlyList<CliConditionallyAvailableCommand> commands)
    {
        foreach (var command in commands)
        {
            if (string.IsNullOrWhiteSpace(command.Command) || string.IsNullOrWhiteSpace(command.Reason))
            {
                throw new InvalidOperationException(
                    "Every conditionally available command requires a full command and a non-empty reason.");
            }
        }

        var duplicate = commands
            .GroupBy(command => NormalizeCommand(command.Command), StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
        {
            throw new InvalidOperationException($"Duplicate conditionally available command: {duplicate.Key}");
        }

        return commands
            .Select(command => command with { Command = NormalizeCommand(command.Command) })
            .OrderBy(command => command.Command, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static IReadOnlyList<string> NormalizeCommands(IEnumerable<string> commands) =>
        commands
            .Select(NormalizeCommand)
            .Where(command => command.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(command => command, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private static IReadOnlyList<string> GetCoverageCommands(CliToolDefinition tool)
        => NormalizeCommands(tool.Commands.Select(command => command.FullCommand));

    private static bool IsSameOrChildOf(string ancestor, string command) =>
        string.Equals(ancestor, command, StringComparison.OrdinalIgnoreCase)
        || command.StartsWith(ancestor + " ", StringComparison.OrdinalIgnoreCase);

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

    internal static string GetManifestPath(CliToolDefinition tool, string outputDirectory) =>
        Path.Combine(
            outputDirectory,
            tool.OutputDirectory,
            "Generated",
            $"{tool.NamespacePrefix}.CommandCoverage.json");

    internal static IReadOnlySet<string> GetBaselineCommandGroups(
        CliToolDefinition tool,
        string outputDirectory) =>
        ReadBaseline(
                tool,
                outputDirectory,
                GetManifestPath(tool, outputDirectory),
                fallbackManifestPath: null,
                pathComparer: StringComparer.OrdinalIgnoreCase,
                allowMissingManifest: false)
            ?.CommandGroups
            .ToHashSet(StringComparer.OrdinalIgnoreCase)
        ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
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

    public int? PreviousCommandCount { get; init; }

    public string? PreviousToolVersion { get; init; }

    public required IReadOnlyList<string> AddedCommands { get; init; }

    public required IReadOnlyList<string> RemovedCommands { get; init; }

    /// <summary>
    /// Command paths whose help invocation never produced a real response (timed out after every
    /// retry, or rejected by the circuit breaker). They and the baseline commands beneath them are
    /// excluded from <see cref="RemovedCommands"/> because the scrape, not the tool, is missing them.
    /// </summary>
    public IReadOnlyList<string> UnavailableCommands { get; init; } = [];

    public required IReadOnlyList<string> KnownGroupsWithoutChildren { get; init; }

    public required IReadOnlyList<string> Violations { get; init; }

    public required bool ChangesApproved { get; init; }
}
