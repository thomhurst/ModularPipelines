using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "delete")]
public record GcloudSpannerDatabasesDeleteOptions(
[property: CliArgument] string Database,
[property: CliArgument] string Instance
) : GcloudOptions;