using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("restore-point", "collection", "list")]
public record AzRestorePointCollectionListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;