using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public record BashFileOptions([property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string FilePath) : BashOptions;
