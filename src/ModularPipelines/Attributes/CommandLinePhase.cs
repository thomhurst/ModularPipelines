namespace ModularPipelines.Attributes;

/// <summary>
/// Defines the semantic rendering phase for a command-line part.
/// </summary>
public enum CommandLinePhase
{
    // These ordinals are emitted by older generated packages. Value 2 was retired
    // with EndOfOptions, so existing values must not move and new phases must not reuse it.

    /// <summary>
    /// A positional operand rendered after the complete command chain and before regular flags and options.
    /// </summary>
    EarlyOperand = 0,

    /// <summary>
    /// Regular flags and options.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// A final option that must follow regular options and positional operands.
    /// </summary>
    Terminal = 4,

    /// <summary>
    /// Positional or pass-through values rendered after option parsing and before terminal options.
    /// </summary>
    Passthrough = 3,
}
