using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventhubs", "namespace", "encryption", "remove")]
public record AzEventhubsNamespaceEncryptionRemoveOptions(
[property: CliOption("--encryption-config")] string EncryptionConfig,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;