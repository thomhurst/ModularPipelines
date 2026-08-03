using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public partial record BashFileOptions([property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string FilePath) : BashOptions;