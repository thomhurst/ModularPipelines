namespace ModularPipelines.Attributes;

/// <summary>
/// Defines the semantic rendering phase for a command-line part.
/// </summary>
public enum CommandLinePhase
{
    /// <summary>
    /// Regular flags and options.
    /// </summary>
    Normal,

    /// <summary>
    /// A final option that must follow regular options and positional operands.
    /// </summary>
    Terminal,

    /// <summary>
    /// Positional or pass-through values rendered after option parsing and before terminal options.
    /// </summary>
    Passthrough,
}
