using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "backups", "set-iam-policy")]
public record GcloudSpannerBackupsSetIamPolicyOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Instance,
[property: CliArgument] string PolicyFile
) : GcloudOptions;