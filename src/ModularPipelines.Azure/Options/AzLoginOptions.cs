using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("login")]
public record AzLoginOptions : AzOptions
{
    [CliFlag("--allow-no-subscriptions")]
    public bool? AllowNoSubscriptions { get; set; }

    [CliOption("--federated-token")]
    public string? FederatedToken { get; set; }

    [CliFlag("--identity")]
    public bool? Identity { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--service-principal")]
    public string? ServicePrincipal { get; set; }

    [CliOption("--tenant")]
    public string? Tenant { get; set; }

    [CliFlag("--use-cert-sn-issuer")]
    public bool? UseCertSnIssuer { get; set; }

    [CliFlag("--use-device-code")]
    public bool? UseDeviceCode { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}