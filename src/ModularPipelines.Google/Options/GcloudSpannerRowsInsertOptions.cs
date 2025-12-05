using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "rows", "insert")]
public record GcloudSpannerRowsInsertOptions(
[property: CliOption("--data")] string[] Data,
[property: CliOption("--table")] string Table,
[property: CliOption("--database")] string Database,
[property: CliOption("--instance")] string Instance
) : GcloudOptions;