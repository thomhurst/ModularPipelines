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
    /// An option that terminates normal option parsing and must follow regular options.
    /// </summary>
    Terminal,

    /// <summary>
    /// An explicit end-of-options marker such as <c>--</c>.
    /// </summary>
    EndOfOptions,

    /// <summary>
    /// Positional or pass-through values that must follow option parsing.
    /// </summary>
    Passthrough,
}
