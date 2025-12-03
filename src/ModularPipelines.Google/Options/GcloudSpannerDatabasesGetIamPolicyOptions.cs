using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "get-iam-policy")]
public record GcloudSpannerDatabasesGetIamPolicyOptions(
[property: CliArgument] string Database,
[property: CliArgument] string Instance
) : GcloudOptions;