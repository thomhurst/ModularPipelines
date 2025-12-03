using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("trust", "signer", "add")]
[ExcludeFromCodeCoverage]
public record DockerTrustSignerAddOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Repository { get; set; }

    [CliOption("--key")]
    public virtual string? Key { get; set; }
}
