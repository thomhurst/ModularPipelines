using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "show-usage")]
public record AzSqlShowUsageOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--usage")] string Usage
) : AzOptions;