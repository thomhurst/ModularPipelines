using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "push")]
[ExcludeFromCodeCoverage]
public record DockerComposePushOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [CommandSwitch("--ignore-push-failures")]
    public virtual string? IgnorePushFailures { get; set; }

    [CommandSwitch("--include-deps")]
    public virtual string? IncludeDeps { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
