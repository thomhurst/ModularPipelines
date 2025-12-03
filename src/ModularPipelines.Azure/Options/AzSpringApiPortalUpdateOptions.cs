using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "api-portal", "update")]
public record AzSpringApiPortalUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliFlag("--assign-endpoint")]
    public bool? AssignEndpoint { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliFlag("--enable-api-try-out")]
    public bool? EnableApiTryOut { get; set; }

    [CliFlag("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--issuer-uri")]
    public string? IssuerUri { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}