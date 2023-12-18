using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("login")]
public record AzLoginOptions : AzOptions
{
    [BooleanCommandSwitch("--allow-no-subscriptions")]
    public bool? AllowNoSubscriptions { get; set; }

    [CommandSwitch("--federated-token")]
    public string? FederatedToken { get; set; }

    [BooleanCommandSwitch("--identity")]
    public bool? Identity { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--service-principal")]
    public string? ServicePrincipal { get; set; }

    [CommandSwitch("--tenant")]
    public string? Tenant { get; set; }

    [BooleanCommandSwitch("--use-cert-sn-issuer")]
    public bool? UseCertSnIssuer { get; set; }

    [BooleanCommandSwitch("--use-device-code")]
    public bool? UseDeviceCode { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}