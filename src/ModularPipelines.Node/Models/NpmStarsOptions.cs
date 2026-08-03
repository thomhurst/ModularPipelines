using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("stars")]
public record NpmStarsOptions : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? User { get; set; }
}