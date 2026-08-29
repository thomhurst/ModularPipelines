using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// Options for executing a Bash script file.
/// </summary>
/// <param name="FilePath">The path to the Bash script file to execute.</param>
[ExcludeFromCodeCoverage]
public record BashFileOptions([property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string FilePath) : BashOptions;
