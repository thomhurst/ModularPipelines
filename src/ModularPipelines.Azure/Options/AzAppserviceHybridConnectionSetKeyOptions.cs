using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appservice", "hybrid-connection", "set-key")]
public record AzAppserviceHybridConnectionSetKeyOptions(
[property: CliOption("--hybrid-connection")] string HybridConnection,
[property: CliOption("--key-type")] string KeyType,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--plan")] string Plan,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;