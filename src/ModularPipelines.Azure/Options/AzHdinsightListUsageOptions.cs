using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight", "list-usage")]
public record AzHdinsightListUsageOptions(
[property: CliOption("--location")] string Location
) : AzOptions;