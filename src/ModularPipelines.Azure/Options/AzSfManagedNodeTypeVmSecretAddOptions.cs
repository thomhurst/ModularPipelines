using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "managed-node-type", "vm-secret", "add")]
public record AzSfManagedNodeTypeVmSecretAddOptions(
[property: CliOption("--certificate-store")] string CertificateStore,
[property: CliOption("--certificate-url")] string CertificateUrl,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source-vault-id")] string SourceVaultId
) : AzOptions;