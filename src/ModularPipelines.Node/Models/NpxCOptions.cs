using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("npx", "-c")]
public record NpxCOptions : NpmOptions
{
    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Cmd { get; set; }
}