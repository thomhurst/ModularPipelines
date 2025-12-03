using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "cluster", "create")]
public record AzSfClusterCreateOptions(
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

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--cluster-size")]
    public string? ClusterSize { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--os")]
    public string? Os { get; set; }

    [CliOption("--parameter-file")]
    public string? ParameterFile { get; set; }

    [CliOption("--secret-identifier")]
    public string? SecretIdentifier { get; set; }

    [CliOption("--template-file")]
    public string? TemplateFile { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }

    [CliOption("--vault-rg")]
    public string? VaultRg { get; set; }

    [CliOption("--vm-password")]
    public string? VmPassword { get; set; }

    [CliOption("--vm-sku")]
    public string? VmSku { get; set; }

    [CliOption("--vm-user-name")]
    public string? VmUserName { get; set; }
}