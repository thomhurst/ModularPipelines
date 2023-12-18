using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace", "encryption", "remove")]
public record AzEventhubsNamespaceEncryptionRemoveOptions(
[property: CommandSwitch("--encryption-config")] string EncryptionConfig,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;