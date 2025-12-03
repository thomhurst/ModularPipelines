using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "ingress", "cors", "update")]
public record AzContainerappIngressCorsUpdateOptions : AzOptions
{
    [CliFlag("--allow-credentials")]
    public bool? AllowCredentials { get; set; }

    [CliFlag("--allowed-headers")]
    public bool? AllowedHeaders { get; set; }

    [CliFlag("--allowed-methods")]
    public bool? AllowedMethods { get; set; }

    [CliFlag("--allowed-origins")]
    public bool? AllowedOrigins { get; set; }

    [CliOption("--expose-headers")]
    public string? ExposeHeaders { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--max-age")]
    public string? MaxAge { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}