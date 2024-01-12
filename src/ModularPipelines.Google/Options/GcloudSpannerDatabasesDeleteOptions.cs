using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "databases", "delete")]
public record GcloudSpannerDatabasesDeleteOptions(
[property: PositionalArgument] string Database,
[property: PositionalArgument] string Instance
) : GcloudOptions;