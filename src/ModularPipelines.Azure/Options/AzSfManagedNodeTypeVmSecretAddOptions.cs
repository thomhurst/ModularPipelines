using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-node-type", "vm-secret", "add")]
public record AzSfManagedNodeTypeVmSecretAddOptions(
[property: CommandSwitch("--certificate-store")] string CertificateStore,
[property: CommandSwitch("--certificate-url")] string CertificateUrl,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--source-vault-id")] string SourceVaultId
) : AzOptions
{
}

