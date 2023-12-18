using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace", "encryption", "add")]
public record AzEventhubsNamespaceEncryptionAddOptions(
[property: CommandSwitch("--encryption-config")] string EncryptionConfig,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--infra-encryption")]
    public bool? InfraEncryption { get; set; }
}

