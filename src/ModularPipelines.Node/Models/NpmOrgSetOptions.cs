using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("org", "set")]
public record NpmOrgSetOptions
(
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string OrgName,
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Username
) : NpmOptions
{
    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? PermissionLevel { get; set; }

    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--parseable")]
    public virtual bool? Parseable { get; set; }
}
