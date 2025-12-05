using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("restore-point", "collection", "list-all")]
public record AzRestorePointCollectionListAllOptions : AzOptions;