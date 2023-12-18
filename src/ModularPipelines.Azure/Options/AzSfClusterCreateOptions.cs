using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "cluster", "create")]
public record AzSfClusterCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--cert-out-folder")]
    public string? CertOutFolder { get; set; }

    [CommandSwitch("--cert-subject-name")]
    public string? CertSubjectName { get; set; }

    [CommandSwitch("--certificate-file")]
    public string? CertificateFile { get; set; }

    [CommandSwitch("--certificate-password")]
    public string? CertificatePassword { get; set; }

    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [CommandSwitch("--cluster-size")]
    public string? ClusterSize { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--os")]
    public string? Os { get; set; }

    [CommandSwitch("--parameter-file")]
    public string? ParameterFile { get; set; }

    [CommandSwitch("--secret-identifier")]
    public string? SecretIdentifier { get; set; }

    [CommandSwitch("--template-file")]
    public string? TemplateFile { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }

    [CommandSwitch("--vault-rg")]
    public string? VaultRg { get; set; }

    [CommandSwitch("--vm-password")]
    public string? VmPassword { get; set; }

    [CommandSwitch("--vm-sku")]
    public string? VmSku { get; set; }

    [CommandSwitch("--vm-user-name")]
    public string? VmUserName { get; set; }
}