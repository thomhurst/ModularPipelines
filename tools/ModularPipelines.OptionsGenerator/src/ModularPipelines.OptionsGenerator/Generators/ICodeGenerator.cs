using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Base interface for all code generators.
/// </summary>
public interface ICodeGenerator
{
    /// <summary>
    /// Generates code files for the given tool definition.
    /// </summary>
    Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default);
}

/// <summary>
/// Describes generated output that can be reconciled after a successful run.
/// </summary>
public interface IGeneratedFileCleanupProvider
{
    /// <summary>
    /// Returns cleanup rules based on every tool currently registered with the generator.
    /// </summary>
    IEnumerable<GeneratedFileCleanupRule> GetCleanupRules(IReadOnlyCollection<string> toolNames);
}

/// <summary>
/// Defines one directory of generated files and the names that should remain there.
/// </summary>
public record GeneratedFileCleanupRule
{
    public required string RelativeDirectory { get; init; }

    public required string SearchPattern { get; init; }

    public required string GeneratedMarker { get; init; }

    public required IReadOnlySet<string> ExpectedFileNames { get; init; }
}

/// <summary>
/// Represents a generated file.
/// </summary>
public record GeneratedFile
{
    /// <summary>
    /// The relative path for the file.
    /// </summary>
    public required string RelativePath { get; init; }

    /// <summary>
    /// The file content.
    /// </summary>
    public required string Content { get; init; }
}
