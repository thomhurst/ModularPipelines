using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("-c")]
public record NpxCOptions : NpxOptions
{
    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Cmd { get; set; }
}
