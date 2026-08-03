namespace ModularPipelines.Attributes;

/// <summary>
/// Defines the semantic rendering phase for a command-line part.
/// </summary>
public enum CommandLinePhase
{
    /// <summary>
    /// A positional operand rendered after the complete command chain and before regular flags and options.
    /// </summary>
    EarlyOperand,

    /// <summary>
    /// Regular flags and options.
    /// </summary>
    Normal,

    /// <summary>
    /// An explicit end-of-options marker such as <c>--</c>, rendered before pass-through operands.
    /// </summary>
    EndOfOptions,

    /// <summary>
    /// Positional or pass-through values rendered after option parsing and before terminal options.
    /// </summary>
    Passthrough,

    /// <summary>
    /// A final option that must follow regular options and positional operands.
    /// </summary>
    Terminal,
}
