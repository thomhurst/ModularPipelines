using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("team", "destroy")]
public record NpmTeamDestroyOptions(
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Scope,
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Otpcode
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
}