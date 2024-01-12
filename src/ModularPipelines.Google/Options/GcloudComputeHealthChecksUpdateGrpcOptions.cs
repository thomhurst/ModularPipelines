using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "health-checks", "update", "grpc")]
public record GcloudComputeHealthChecksUpdateGrpcOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--check-interval")]
    public string? CheckInterval { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CommandSwitch("--grpc-service-name")]
    public string? GrpcServiceName { get; set; }

    [CommandSwitch("--healthy-threshold")]
    public string? HealthyThreshold { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--unhealthy-threshold")]
    public string? UnhealthyThreshold { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [BooleanCommandSwitch("--use-serving-port")]
    public bool? UseServingPort { get; set; }
}