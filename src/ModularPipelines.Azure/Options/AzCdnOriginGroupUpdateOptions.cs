using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "origin-group", "update")]
public record AzCdnOriginGroupUpdateOptions : AzOptions
{
    [CommandSwitch("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--origins")]
    public string? Origins { get; set; }

    [CommandSwitch("--probe-interval")]
    public string? ProbeInterval { get; set; }

    [CommandSwitch("--probe-method")]
    public string? ProbeMethod { get; set; }

    [CommandSwitch("--probe-path")]
    public string? ProbePath { get; set; }

    [CommandSwitch("--probe-protocol")]
    public string? ProbeProtocol { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}