namespace ModularPipelines.Attributes;

/// <summary>
/// Defines the semantic rendering phase for a command-line part.
/// </summary>
public enum CommandLinePhase
{
    // These ordinals are emitted by generated packages, so existing values must not move.

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

    /// <summary>
    /// A positional operand rendered after pass-through values and before terminal options.
    /// </summary>
    LateOperand = 5,
}

internal static class CommandLinePhaseOrder
{
    internal static int GetRenderOrder(CommandLinePhase phase) => phase switch
    {
        CommandLinePhase.EarlyOperand => 0,
        CommandLinePhase.Normal => 1,
        CommandLinePhase.Passthrough => 3,
        CommandLinePhase.LateOperand => 4,
        CommandLinePhase.Terminal => 5,
        _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null),
    };
}
