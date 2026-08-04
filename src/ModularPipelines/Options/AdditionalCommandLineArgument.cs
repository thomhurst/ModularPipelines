using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// A manually supplied command-line token with explicit placement metadata.
/// </summary>
/// <param name="Value">The token to add to the command line.</param>
/// <param name="Phase">The semantic rendering phase for the token.</param>
/// <param name="IsGlobalOption">
/// Whether the token belongs before the command or subcommand parts.
/// </param>
public sealed record AdditionalCommandLineArgument(
    string Value,
    CommandLinePhase Phase = CommandLinePhase.Normal,
    bool IsGlobalOption = false);
