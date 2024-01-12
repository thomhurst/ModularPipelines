using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "databases", "get-iam-policy")]
public record GcloudSpannerDatabasesGetIamPolicyOptions(
[property: PositionalArgument] string Database,
[property: PositionalArgument] string Instance
) : GcloudOptions;