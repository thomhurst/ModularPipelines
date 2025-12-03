using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("org", "rm", "orgname", "username")]
public record NpmOrgRmOptions
(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string OrgName,
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Username
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
}