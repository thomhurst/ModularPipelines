namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Represents a complete CLI tool definition with all its commands.
/// </summary>
public record CliToolDefinition
{
    /// <summary>
    /// Stable identity used to reconcile externally generated files across mutable
    /// tool names, namespace prefixes, and output directories.
    /// </summary>
    public string? OwnershipId { get; init; }

    /// <summary>
    /// The tool name (e.g., "kubectl", "docker", "az").
    /// </summary>
    public required string ToolName { get; init; }

    /// <summary>
    /// The namespace prefix for generated classes (e.g., "Kubernetes", "Docker", "Azure").
    /// </summary>
    public required string NamespacePrefix { get; init; }

    /// <summary>
    /// The target namespace for generated options (e.g., "ModularPipelines.Kubernetes").
    /// </summary>
    public required string TargetNamespace { get; init; }

    /// <summary>
    /// The output directory relative to the repository root.
    /// </summary>
    public required string OutputDirectory { get; init; }

    /// <summary>
    /// Optional documentation output directory relative to the selected output root.
    /// Set to <see langword="null"/> to omit generated documentation.
    /// </summary>
    public string? DocumentationOutputDirectory { get; init; } =
        Path.Combine("docs", "docs", "mp-packages", "cli");

    /// <summary>
    /// Gets or sets a value indicating whether to generate the root command facade,
    /// its implementation, and its dependency-injection registration.
    /// </summary>
    public bool GenerateCommandFacade { get; init; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether code and documentation artifacts should be generated.
    /// Disable this for integrations that are intentionally maintained by hand but still participate
    /// in scraping and command-coverage validation.
    /// </summary>
    public bool GenerateCode { get; init; } = true;

    /// <summary>
    /// All commands for this tool.
    /// </summary>
    public required IReadOnlyList<CliCommandDefinition> Commands { get; init; }

    /// <summary>
    /// Version reported by the installed CLI, when available.
    /// </summary>
    public string? ToolVersion { get; init; }

    /// <summary>
    /// Tool-specific invariants used by the shared command coverage guard.
    /// </summary>
    public CliCommandCoveragePolicy CommandCoverage { get; init; } = new();

    /// <summary>
    /// Global options that apply to all commands.
    /// </summary>
    public IReadOnlyList<CliOptionDefinition> GlobalOptions { get; init; } = [];

    /// <summary>
    /// Global options absent from CLI help because they depend on an edition, license, plugin,
    /// or environment. These use the same metadata and generation path as scraped options.
    /// </summary>
    public IReadOnlyList<CliOptionDefinition> SupplementalGlobalOptions { get; init; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether inherited tool-wide options are emitted before
    /// subcommands. Some CLIs describe options as global while accepting them only after the
    /// selected command path.
    /// </summary>
    public bool GlobalOptionsBeforeSubcommands { get; init; } = true;

    /// <summary>
    /// Returns the validated, deterministic union of scraped and supplemental global options.
    /// </summary>
    public IReadOnlyList<CliOptionDefinition> GetGlobalOptions() =>
        CliGlobalOptionMerger.Merge(GlobalOptions, SupplementalGlobalOptions);

    /// <summary>
    /// Full command name selected for the runnable documentation example.
    /// A missing value deliberately omits the runnable example.
    /// </summary>
    public string? PreferredDocumentationExampleCommand { get; init; }

    /// <summary>
    /// Installation metadata for the external executable used by this integration.
    /// </summary>
    public CliExecutablePrerequisite? ExecutablePrerequisite { get; init; }

    /// <summary>
    /// Explicit reason this tool uses generic prerequisite guidance instead of
    /// tool-specific installation metadata.
    /// </summary>
    public string? ExecutablePrerequisiteMetadataExemption { get; init; }

    /// <summary>
    /// Sub-domain groups for organizing commands (e.g., "Container", "Image" for Docker).
    /// </summary>
    public IReadOnlyList<string> SubDomainGroups => Commands
        .Where(c => c.SubDomainGroup is not null)
        .Select(c => c.SubDomainGroup!)
        .Distinct()
        .OrderBy(g => g)
        .ToList();

    /// <summary>
    /// All enums that need to be generated for this tool.
    /// Deduplication is deliberately case-SENSITIVE: names differing only by case are a
    /// scraper casing bug, and dropping one here would leave options referencing the
    /// dropped spelling (C# type lookup is case-sensitive). Case-variant names flow
    /// through to the generated file paths, where the orchestrator's duplicate-path
    /// check fails the tool loudly instead.
    /// </summary>
    public IReadOnlyList<CliEnumDefinition> AllEnums => Commands
        .SelectMany(command => command.Enums.Concat(
            command.Options
                .Where(option => option.EnumDefinition is not null)
                .Select(option => option.EnumDefinition!)))
        .Concat(GetGlobalOptions()
            .Where(option => option.EnumDefinition is not null)
            .Select(option => option.EnumDefinition!))
        .DistinctBy(e => e.EnumName)
        .ToList();

    /// <summary>
    /// Any scraping errors encountered.
    /// </summary>
    public IReadOnlyList<ScrapingError> Errors { get; init; } = [];
}

/// <summary>
/// Configures command coverage invariants for a CLI tool.
/// </summary>
public record CliCommandCoveragePolicy
{
    /// <summary>
    /// Minimum number of distinct commands that a successful scrape must contain.
    /// </summary>
    public int? MinimumCommandCount { get; init; }

    /// <summary>
    /// Commands that must remain discoverable unless they have a documented exclusion.
    /// </summary>
    public IReadOnlyList<string> SentinelCommands { get; init; } = [];

    /// <summary>
    /// Commands whose help visibility depends on an edition, license, plugin, or environment.
    /// </summary>
    public IReadOnlyList<CliConditionallyAvailableCommand> ConditionallyAvailableCommands { get; init; } = [];

    /// <summary>
    /// Commands intentionally omitted from generation, with machine-readable reasons.
    /// </summary>
    public IReadOnlyList<CliCommandCoverageExclusion> Exclusions { get; init; } = [];
}

/// <summary>
/// Documents a command whose visibility in CLI help depends on the current installation.
/// </summary>
public record CliConditionallyAvailableCommand
{
    /// <summary>
    /// Full CLI command path.
    /// </summary>
    public required string Command { get; init; }

    /// <summary>
    /// Human-readable availability condition, such as an edition, license, plugin, or environment.
    /// </summary>
    public required string Reason { get; init; }
}

/// <summary>
/// Documents why a command is intentionally excluded from generated output.
/// </summary>
public record CliCommandCoverageExclusion
{
    /// <summary>
    /// Full CLI command path.
    /// </summary>
    public required string Command { get; init; }

    /// <summary>
    /// Human-readable reason, such as edition, licensing, or unsupported syntax.
    /// </summary>
    public required string Reason { get; init; }
}

/// <summary>
/// Describes the external executable required by a generated CLI integration.
/// </summary>
public record CliExecutablePrerequisite
{
    /// <summary>
    /// Executable command that must resolve through PATH.
    /// </summary>
    public required string CommandName { get; init; }

    /// <summary>
    /// Supported or workflow-pinned executable version, when generation uses one.
    /// </summary>
    public string? SupportedVersion { get; init; }

    /// <summary>
    /// Official installation documentation URL.
    /// </summary>
    public string? InstallationUrl { get; init; }

    /// <summary>
    /// Optional installation or compatibility notes.
    /// </summary>
    public string? InstallationNotes { get; init; }
}

/// <summary>
/// Represents an error that occurred during scraping.
/// </summary>
public record ScrapingError
{
    /// <summary>
    /// The URL or command that failed.
    /// </summary>
    public required string Source { get; init; }

    /// <summary>
    /// The error message.
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// The exception if available.
    /// </summary>
    public Exception? Exception { get; init; }
}
