using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hdinsight", "list-usage")]
public record AzHdinsightListUsageOptions(
[property: CliOption("--location")] string Location
) : AzOptions;