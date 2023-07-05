using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

public record DockerPushOptions( string Name, string Tag ) : DockerOptions
{
    [BooleanCommandSwitch( "disable-content-trust" )]
    public bool DisableContentTrust { get; init; } = true;
};
