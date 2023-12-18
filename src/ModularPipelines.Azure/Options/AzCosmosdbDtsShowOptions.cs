using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "dts", "show")]
public record AzCosmosdbDtsShowOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;