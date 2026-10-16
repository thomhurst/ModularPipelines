namespace ModularPipelines.Context;

/// <summary>
/// The severity of an annotation published to a CI build system.
/// </summary>
public enum BuildAnnotationLevel
{
    /// <summary>An informational notice.</summary>
    Notice,

    /// <summary>A warning.</summary>
    Warning,

    /// <summary>An error.</summary>
    Error,
}
