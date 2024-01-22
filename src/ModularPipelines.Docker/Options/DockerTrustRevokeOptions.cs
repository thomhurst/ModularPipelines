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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Image { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}
