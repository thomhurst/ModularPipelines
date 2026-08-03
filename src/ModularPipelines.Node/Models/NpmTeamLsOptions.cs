using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("team", "ls")]
public record NpmTeamLsOptions(
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Value
) : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }

    [CliFlag("--parseable")]
    public virtual bool? Parseable { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliArgument(Phase = CommandLinePhase.Passthrough)]
    public virtual string? Scope { get; set; }
}
