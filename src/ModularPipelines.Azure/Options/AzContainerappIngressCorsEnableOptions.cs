using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "ingress", "cors", "enable")]
public record AzContainerappIngressCorsEnableOptions(
[property: CliFlag("--allowed-origins")] bool AllowedOrigins
) : AzOptions
{
    [CliFlag("--allow-credentials")]
    public bool? AllowCredentials { get; set; }

    [CliFlag("--allowed-headers")]
    public bool? AllowedHeaders { get; set; }

    [CliFlag("--allowed-methods")]
    public bool? AllowedMethods { get; set; }

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