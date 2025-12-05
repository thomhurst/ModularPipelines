using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("org", "ls", "orgname")]
public record NpmOrgLsOptions
(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string OrgName
) : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--parseable")]
    public virtual bool? Parseable { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public virtual string? Username { get; set; }
}