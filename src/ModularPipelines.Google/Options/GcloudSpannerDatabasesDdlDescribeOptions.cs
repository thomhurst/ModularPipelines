using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "ddl", "describe")]
public record GcloudSpannerDatabasesDdlDescribeOptions(
[property: CliArgument] string Database,
[property: CliArgument] string Instance
) : GcloudOptions;