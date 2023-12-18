using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "gateway", "update")]
public record AzSpringGatewayUpdateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--addon-configs-file")]
    public string? AddonConfigsFile { get; set; }

    [CommandSwitch("--addon-configs-json")]
    public string? AddonConfigsJson { get; set; }

    [BooleanCommandSwitch("--allow-credentials")]
    public bool? AllowCredentials { get; set; }

    [BooleanCommandSwitch("--allow-origin-patterns")]
    public bool? AllowOriginPatterns { get; set; }

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

    [CommandSwitch("--apm-types")]
    public string? ApmTypes { get; set; }

    [CommandSwitch("--apms")]
    public string? Apms { get; set; }

    [BooleanCommandSwitch("--assign-endpoint")]
    public bool? AssignEndpoint { get; set; }

    [CommandSwitch("--certificate-names")]
    public string? CertificateNames { get; set; }

    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--client-secret")]
    public string? ClientSecret { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [BooleanCommandSwitch("--enable-cert-verify")]
    public bool? EnableCertVerify { get; set; }

    [BooleanCommandSwitch("--enable-response-cache")]
    public bool? EnableResponseCache { get; set; }

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

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [CommandSwitch("--response-cache-scope")]
    public string? ResponseCacheScope { get; set; }

    [CommandSwitch("--response-cache-size")]
    public string? ResponseCacheSize { get; set; }

    [CommandSwitch("--response-cache-ttl")]
    public string? ResponseCacheTtl { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--secrets")]
    public string? Secrets { get; set; }

    [CommandSwitch("--server-url")]
    public string? ServerUrl { get; set; }
}