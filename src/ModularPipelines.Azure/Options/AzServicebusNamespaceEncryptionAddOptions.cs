using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "namespace", "encryption", "add")]
public record AzServicebusNamespaceEncryptionAddOptions(
[property: CommandSwitch("--encryption-config")] string EncryptionConfig,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--infra-encryption")]
    public bool? InfraEncryption { get; set; }
}