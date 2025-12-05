using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "log", "config")]
public record AzWebappLogConfigOptions : AzOptions
{
    [CliOption("--application-logging")]
    public string? ApplicationLogging { get; set; }

    [CliFlag("--detailed-error-messages")]
    public bool? DetailedErrorMessages { get; set; }

    [CliOption("--docker-container-logging")]
    public string? DockerContainerLogging { get; set; }

    [CliFlag("--failed-request-tracing")]
    public bool? FailedRequestTracing { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--level")]
    public string? Level { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--web-server-logging")]
    public string? WebServerLogging { get; set; }
}