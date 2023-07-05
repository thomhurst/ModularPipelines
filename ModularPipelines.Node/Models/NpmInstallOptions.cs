using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Node.Models;

public record NpmInstallOptions : CommandLineOptions
{
    public string? Target { get; init; }

    [BooleanCommandSwitch( "global" )]
    public bool Global { get; init; }

    [BooleanCommandSwitch( "dry-run" )]
    public bool DryRun { get; init; }

    [BooleanCommandSwitch( "force" )]
    public bool Force { get; init; }
}
