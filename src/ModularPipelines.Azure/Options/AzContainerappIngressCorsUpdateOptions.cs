using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "ingress", "cors", "update")]
public record AzContainerappIngressCorsUpdateOptions : AzOptions
{
    [BooleanCommandSwitch("--allow-credentials")]
    public bool? AllowCredentials { get; set; }

    [BooleanCommandSwitch("--allowed-headers")]
    public bool? AllowedHeaders { get; set; }

    [BooleanCommandSwitch("--allowed-methods")]
    public bool? AllowedMethods { get; set; }

    [BooleanCommandSwitch("--allowed-origins")]
    public bool? AllowedOrigins { get; set; }

    [CommandSwitch("--expose-headers")]
    public string? ExposeHeaders { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--max-age")]
    public string? MaxAge { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}