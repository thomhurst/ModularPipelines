using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "backups", "get-iam-policy")]
public record GcloudSpannerBackupsGetIamPolicyOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Instance
) : GcloudOptions;