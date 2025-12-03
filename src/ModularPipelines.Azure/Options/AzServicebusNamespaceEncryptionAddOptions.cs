using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "namespace", "encryption", "add")]
public record AzServicebusNamespaceEncryptionAddOptions(
[property: CliOption("--encryption-config")] string EncryptionConfig,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--infra-encryption")]
    public bool? InfraEncryption { get; set; }
}