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
