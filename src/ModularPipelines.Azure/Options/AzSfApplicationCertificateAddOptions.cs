using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "application", "certificate", "add")]
public record AzSfApplicationCertificateAddOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
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

    [CommandSwitch("--secret-identifier")]
    public string? SecretIdentifier { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }

    [CommandSwitch("--vault-rg")]
    public string? VaultRg { get; set; }
}