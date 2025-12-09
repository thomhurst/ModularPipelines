using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("nginx", "deployment", "certificate", "create")]
public record AzNginxDeploymentCertificateCreateOptions(
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliOption("--deployment-name")] string DeploymentName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CliOption("--key-path")]
    public string? KeyPath { get; set; }

    [CliOption("--key-vault-secret-id")]
    public string? KeyVaultSecretId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}