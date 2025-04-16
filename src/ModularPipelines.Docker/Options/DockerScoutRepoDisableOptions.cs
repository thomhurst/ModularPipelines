using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "repo", "disable")]
[ExcludeFromCodeCoverage]
public record DockerScoutRepoDisableOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Repository { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [CommandSwitch("--integration")]
    public virtual string? Integration { get; set; }

    [CommandSwitch("--org")]
    public virtual string? Org { get; set; }

    [CommandSwitch("--registry")]
    public virtual string? Registry { get; set; }
}
