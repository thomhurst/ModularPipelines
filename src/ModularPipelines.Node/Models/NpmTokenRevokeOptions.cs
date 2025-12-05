using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("token", "revoke")]
public record NpmTokenRevokeOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Value
) : NpmOptions
{
    [CliFlag("--read-only")]
    public virtual bool? ReadOnly { get; set; }

    [CliOption("--cidr")]
    public virtual string? Cidr { get; set; }

    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }
}