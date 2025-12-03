using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "set-iam-policy")]
public record GcloudSpannerDatabasesSetIamPolicyOptions(
[property: CliArgument] string Database,
[property: CliArgument] string Instance,
[property: CliArgument] string PolicyFile
) : GcloudOptions;