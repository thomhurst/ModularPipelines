using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "frontend-endpoint", "enable-https")]
public record AzNetworkFrontDoorFrontendEndpointEnableHttpsOptions(
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--certificate-source")]
    public string? CertificateSource { get; set; }

    [CommandSwitch("--minimum-tls-version")]
    public string? MinimumTlsVersion { get; set; }

    [CommandSwitch("--secret-name")]
    public string? SecretName { get; set; }

    [CommandSwitch("--secret-version")]
    public string? SecretVersion { get; set; }

    [CommandSwitch("--vault-id")]
    public string? VaultId { get; set; }
}