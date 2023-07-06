using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[CommandPrecedingArguments("install")]
public record NpmInstallOptions : NpmOptions
{
    public string? Target { get; init; }

    [BooleanCommandSwitch("global")]
    public bool Global { get; init; }

    [BooleanCommandSwitch("dry-run")]
    public bool DryRun { get; init; }

    [BooleanCommandSwitch("force")]
    public bool Force { get; init; }
}
