using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "databases", "set-iam-policy")]
public record GcloudSpannerDatabasesSetIamPolicyOptions(
[property: PositionalArgument] string Database,
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;