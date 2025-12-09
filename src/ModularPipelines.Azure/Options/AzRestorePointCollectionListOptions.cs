using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("restore-point", "collection", "list")]
public record AzRestorePointCollectionListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;