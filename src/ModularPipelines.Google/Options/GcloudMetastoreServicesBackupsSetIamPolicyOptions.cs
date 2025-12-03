using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "backups", "set-iam-policy")]
public record GcloudMetastoreServicesBackupsSetIamPolicyOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Location,
[property: CliArgument] string Service,
[property: CliArgument] string PolicyFile
) : GcloudOptions;