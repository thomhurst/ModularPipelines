using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerTrustRevokeOptions : DockerOptions
{
    public DockerTrustRevokeOptions(
        string image
    )
    {
        CommandParts = ["trust", "revoke"];

        Image = image;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Image { get; set; }

    [CliFlag("--yes")]
    public virtual bool? Yes { get; set; }
}
