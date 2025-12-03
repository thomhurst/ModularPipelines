using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "rows", "delete")]
public record GcloudSpannerRowsDeleteOptions(
[property: CliOption("--keys")] string[] Keys,
[property: CliOption("--table")] string Table,
[property: CliOption("--database")] string Database,
[property: CliOption("--instance")] string Instance
) : GcloudOptions;