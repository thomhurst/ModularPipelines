using ModularPipelines.Options;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Resolves the CLI tool name from an options type or instance.
/// </summary>
internal interface IToolResolver
{
    /// <summary>
    /// Resolves the tool name from the type's [CliTool] attribute chain.
    /// </summary>
    string? ResolveTool(Type optionsType);

    /// <summary>
    /// Resolves the tool name from an options instance.
    /// First checks [CliTool] attribute, then falls back to Tool property.
    /// </summary>
    string? ResolveTool(CommandLineToolOptions options);
}
