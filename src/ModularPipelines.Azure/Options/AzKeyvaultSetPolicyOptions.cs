using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "set-policy")]
public record AzKeyvaultSetPolicyOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--application-id")]
    public string? ApplicationId { get; set; }

    [CliOption("--certificate-permissions")]
    public string? CertificatePermissions { get; set; }

    [CliOption("--key-permissions")]
    public string? KeyPermissions { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--object-id")]
    public string? ObjectId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secret-permissions")]
    public string? SecretPermissions { get; set; }

    [CliOption("--spn")]
    public string? Spn { get; set; }

    [CliOption("--storage-permissions")]
    public string? StoragePermissions { get; set; }

    [CliOption("--upn")]
    public string? Upn { get; set; }
}