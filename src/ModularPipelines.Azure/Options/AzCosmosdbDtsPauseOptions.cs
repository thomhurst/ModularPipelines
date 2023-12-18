using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "dts", "pause")]
public record AzCosmosdbDtsPauseOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;