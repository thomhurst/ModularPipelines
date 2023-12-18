using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "gateway", "show")]
public record AzSpringCloudGatewayShowOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [BooleanCommandSwitch("--allow-credentials")]
    public bool? AllowCredentials { get; set; }

    [BooleanCommandSwitch("--allowed-headers")]
    public bool? AllowedHeaders { get; set; }

    [BooleanCommandSwitch("--allowed-methods")]
    public bool? AllowedMethods { get; set; }

    [BooleanCommandSwitch("--allowed-origins")]
    public bool? AllowedOrigins { get; set; }

    [CommandSwitch("--api-description")]
    public string? ApiDescription { get; set; }

    [CommandSwitch("--api-doc-location")]
    public string? ApiDocLocation { get; set; }

    [CommandSwitch("--api-title")]
    public string? ApiTitle { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [BooleanCommandSwitch("--assign-endpoint")]
    public bool? AssignEndpoint { get; set; }

    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--client-secret")]
    public string? ClientSecret { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--exposed-headers")]
    public string? ExposedHeaders { get; set; }

    [BooleanCommandSwitch("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--issuer-uri")]
    public string? IssuerUri { get; set; }

    [CommandSwitch("--max-age")]
    public string? MaxAge { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--server-url")]
    public string? ServerUrl { get; set; }
}