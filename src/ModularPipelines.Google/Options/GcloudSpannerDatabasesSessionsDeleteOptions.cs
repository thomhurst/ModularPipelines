using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "sessions", "delete")]
public record GcloudSpannerDatabasesSessionsDeleteOptions(
[property: CliArgument] string Session,
[property: CliArgument] string Database,
[property: CliArgument] string Instance
) : GcloudOptions;