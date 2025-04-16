using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "create")]
[ExcludeFromCodeCoverage]
public record DockerBuildxCreateOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ContextOrEndpoint { get; set; }

    [CommandSwitch("--append")]
    public virtual string? Append { get; set; }

    [CommandSwitch("--bootstrap")]
    public virtual string? Bootstrap { get; set; }

    [CommandSwitch("--builder")]
    public virtual string? Builder { get; set; }

    [CommandSwitch("--buildkitd-flags")]
    public virtual string? BuildkitdFlags { get; set; }

    [CommandSwitch("--driver")]
    public virtual string? Driver { get; set; }

    [CommandSwitch("--driver-opt")]
    public virtual string? DriverOpt { get; set; }

    [CommandSwitch("--leave")]
    public virtual string? Leave { get; set; }

    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--node")]
    public virtual string? Node { get; set; }

    [CommandSwitch("--platform")]
    public virtual string? Platform { get; set; }

    [CommandSwitch("--use")]
    public virtual string? Use { get; set; }
}
