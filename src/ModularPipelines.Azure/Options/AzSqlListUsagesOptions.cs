using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "list-usages")]
public record AzSqlListUsagesOptions(
[property: CliOption("--location")] string Location
) : AzOptions;