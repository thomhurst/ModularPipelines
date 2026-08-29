using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// Options for executing an inline Windows Command Prompt script.
/// </summary>
/// <param name="Script">The script to execute.</param>
[ExcludeFromCodeCoverage]
public record CmdScriptOptions(
    [property: CliArgument(Phase = CommandLinePhase.Passthrough)] string Script) : CmdOptions;
