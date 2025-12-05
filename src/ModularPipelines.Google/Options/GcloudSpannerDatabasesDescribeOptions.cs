using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "describe")]
public record GcloudSpannerDatabasesDescribeOptions(
[property: CliArgument] string Database,
[property: CliArgument] string Instance
) : GcloudOptions;