using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "backups", "get-iam-policy")]
public record GcloudSpannerBackupsGetIamPolicyOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Instance
) : GcloudOptions;