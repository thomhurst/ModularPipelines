using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "databases", "describe")]
public record GcloudSpannerDatabasesDescribeOptions(
[property: PositionalArgument] string Database,
[property: PositionalArgument] string Instance
) : GcloudOptions;