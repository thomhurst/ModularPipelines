using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "gateway", "update")]
public record AzSpringGatewayUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--addon-configs-file")]
    public string? AddonConfigsFile { get; set; }

    [CliOption("--addon-configs-json")]
    public string? AddonConfigsJson { get; set; }

    [CliFlag("--allow-credentials")]
    public bool? AllowCredentials { get; set; }

    [CliFlag("--allow-origin-patterns")]
    public bool? AllowOriginPatterns { get; set; }

    [CliFlag("--allowed-headers")]
    public bool? AllowedHeaders { get; set; }

    [CliFlag("--allowed-methods")]
    public bool? AllowedMethods { get; set; }

    [CliFlag("--allowed-origins")]
    public bool? AllowedOrigins { get; set; }

    [CliOption("--api-description")]
    public string? ApiDescription { get; set; }

    [CliOption("--api-doc-location")]
    public string? ApiDocLocation { get; set; }

    [CliOption("--api-title")]
    public string? ApiTitle { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--apm-types")]
    public string? ApmTypes { get; set; }

    [CliOption("--apms")]
    public string? Apms { get; set; }

    [CliFlag("--assign-endpoint")]
    public bool? AssignEndpoint { get; set; }

    [CliOption("--certificate-names")]
    public string? CertificateNames { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliFlag("--enable-cert-verify")]
    public bool? EnableCertVerify { get; set; }

    [CliFlag("--enable-response-cache")]
    public bool? EnableResponseCache { get; set; }

    [CliOption("--exposed-headers")]
    public string? ExposedHeaders { get; set; }

    [CliFlag("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--issuer-uri")]
    public string? IssuerUri { get; set; }

    [CliOption("--max-age")]
    public string? MaxAge { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliOption("--response-cache-scope")]
    public string? ResponseCacheScope { get; set; }

    [CliOption("--response-cache-size")]
    public string? ResponseCacheSize { get; set; }

    [CliOption("--response-cache-ttl")]
    public string? ResponseCacheTtl { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--secrets")]
    public string? Secrets { get; set; }

    [CliOption("--server-url")]
    public string? ServerUrl { get; set; }
}