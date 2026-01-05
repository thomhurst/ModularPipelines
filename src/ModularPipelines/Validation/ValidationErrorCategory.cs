namespace ModularPipelines.Validation;

/// <summary>
/// Categorizes the type of validation error.
/// </summary>
public enum ValidationErrorCategory
{
    /// <summary>
    /// Error related to pipeline options configuration.
    /// </summary>
    Options,

    /// <summary>
    /// Error related to module configuration.
    /// </summary>
    ModuleConfiguration,

    /// <summary>
    /// Error related to module dependencies (circular, missing, self-reference).
    /// </summary>
    Dependency,

    /// <summary>
    /// Error related to type compatibility between modules.
    /// </summary>
    TypeCompatibility,

    /// <summary>
    /// Error related to pipeline requirements.
    /// </summary>
    Requirement,
}
