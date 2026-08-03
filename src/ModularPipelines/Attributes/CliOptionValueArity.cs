namespace ModularPipelines.Attributes;

/// <summary>
/// Describes whether a CLI option requires a value.
/// </summary>
public enum CliOptionValueArity
{
    /// <summary>
    /// The option requires a value.
    /// </summary>
    Required,

    /// <summary>
    /// The option can be rendered either bare or with a value.
    /// A null property omits the option; <see cref="ModularPipelines.Models.CliOptionValue.Bare"/> renders it bare.
    /// </summary>
    Optional,
}
