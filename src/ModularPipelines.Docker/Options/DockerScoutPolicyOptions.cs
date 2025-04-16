using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "policy")]
[ExcludeFromCodeCoverage]
public record DockerScoutPolicyOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ImageOrRepo { get; set; }

    [CommandSwitch("--env")]
    public virtual string? Env { get; set; }

    [CommandSwitch("--exit-code")]
    public virtual string? ExitCode { get; set; }

    [CommandSwitch("--org")]
    public virtual string? Org { get; set; }

    [CommandSwitch("--output")]
    public virtual string? Output { get; set; }

    [CommandSwitch("--platform")]
    public virtual string? Platform { get; set; }

    [CommandSwitch("--to-env")]
    public virtual string? ToEnv { get; set; }

    [CommandSwitch("--to-latest")]
    public virtual string? ToLatest { get; set; }
}
