using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service ps")]
[ExcludeFromCodeCoverage]
public record DockerServicePsOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Service) : DockerOptions
{
    [BooleanCommandSwitch("--no-resolve")]
    public bool? NoResolve { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}
