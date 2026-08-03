using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("edit")]
public record NpmEditOptions : NpmOptions
{
    [CliOption("--editor")]
    public virtual string? Editor { get; set; }

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Pkg { get; set; }

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Subpkg { get; set; }
}