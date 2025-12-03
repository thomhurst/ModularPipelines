using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "application", "certificate", "add")]
public record AzSfApplicationCertificateAddOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cert-out-folder")]
    public string? CertOutFolder { get; set; }

    [CliOption("--cert-subject-name")]
    public string? CertSubjectName { get; set; }

    [CliOption("--certificate-file")]
    public string? CertificateFile { get; set; }

    [CliOption("--certificate-password")]
    public string? CertificatePassword { get; set; }

    [CliOption("--secret-identifier")]
    public string? SecretIdentifier { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }

    [CliOption("--vault-rg")]
    public string? VaultRg { get; set; }
}