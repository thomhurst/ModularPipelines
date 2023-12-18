using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nginx", "deployment", "certificate", "create")]
public record AzNginxDeploymentCertificateCreateOptions(
[property: CommandSwitch("--certificate-name")] string CertificateName,
[property: CommandSwitch("--deployment-name")] string DeploymentName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CommandSwitch("--key-path")]
    public string? KeyPath { get; set; }

    [CommandSwitch("--key-vault-secret-id")]
    public string? KeyVaultSecretId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

