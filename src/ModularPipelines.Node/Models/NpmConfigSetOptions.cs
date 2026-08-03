using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("config", "set")]
public record NpmConfigSetOptions(
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Value
) : NpmOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliOption("--editor")]
    public virtual string? Editor { get; set; }

    [CliOption("--location")]
    public virtual string? Location { get; set; }

    [CliFlag("--long")]
    public virtual bool? Long { get; set; }

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Key { get; set; }
}
