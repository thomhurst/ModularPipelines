using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "backups", "set-iam-policy")]
public record GcloudSpannerBackupsSetIamPolicyOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;