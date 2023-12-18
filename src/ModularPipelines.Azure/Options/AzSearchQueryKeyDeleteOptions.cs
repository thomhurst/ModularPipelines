using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("search", "query-key", "delete")]
public record AzSearchQueryKeyDeleteOptions(
[property: CommandSwitch("--key-value")] string KeyValue,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions;