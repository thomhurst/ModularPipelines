namespace ModularPipelines.Enums;

/// <summary>
/// Output formats supported by dependency graph export.
/// </summary>
public enum DependencyGraphFormat
{
    /// <summary>
    /// Mermaid flowchart markup.
    /// </summary>
    Mermaid,

    /// <summary>
    /// Graphviz DOT markup.
    /// </summary>
    Dot,

    /// <summary>
    /// A machine-readable JSON document.
    /// </summary>
    Json,
}
