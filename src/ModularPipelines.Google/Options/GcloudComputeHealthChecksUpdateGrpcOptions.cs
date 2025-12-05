using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "health-checks", "update", "grpc")]
public record GcloudComputeHealthChecksUpdateGrpcOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--check-interval")]
    public string? CheckInterval { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CliOption("--grpc-service-name")]
    public string? GrpcServiceName { get; set; }

    [CliOption("--healthy-threshold")]
    public string? HealthyThreshold { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--unhealthy-threshold")]
    public string? UnhealthyThreshold { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliFlag("--use-serving-port")]
    public bool? UseServingPort { get; set; }
}