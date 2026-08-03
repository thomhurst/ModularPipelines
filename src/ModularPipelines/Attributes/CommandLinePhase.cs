namespace ModularPipelines.Attributes;

/// <summary>
/// Defines the semantic rendering phase for a command-line part.
/// </summary>
public enum CommandLinePhase
{
    /// <summary>
    /// Regular flags and options.
    /// </summary>
    Normal = 0,

    /// <summary>
    /// A final option that must follow regular options and positional operands.
    /// </summary>
    Terminal = 1,

    /// <summary>
    /// Positional or pass-through values rendered after option parsing and before terminal options.
    /// </summary>
    Passthrough = 3,
}
