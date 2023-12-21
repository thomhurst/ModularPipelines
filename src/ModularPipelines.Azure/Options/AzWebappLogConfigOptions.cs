using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "log", "config")]
public record AzWebappLogConfigOptions : AzOptions
{
    [CommandSwitch("--application-logging")]
    public string? ApplicationLogging { get; set; }

    [BooleanCommandSwitch("--detailed-error-messages")]
    public bool? DetailedErrorMessages { get; set; }

    [CommandSwitch("--docker-container-logging")]
    public string? DockerContainerLogging { get; set; }

    [BooleanCommandSwitch("--failed-request-tracing")]
    public bool? FailedRequestTracing { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--level")]
    public string? Level { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--web-server-logging")]
    public string? WebServerLogging { get; set; }
}