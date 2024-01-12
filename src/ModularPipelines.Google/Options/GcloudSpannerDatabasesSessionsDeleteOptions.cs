using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "databases", "sessions", "delete")]
public record GcloudSpannerDatabasesSessionsDeleteOptions(
[property: PositionalArgument] string Session,
[property: PositionalArgument] string Database,
[property: PositionalArgument] string Instance
) : GcloudOptions;