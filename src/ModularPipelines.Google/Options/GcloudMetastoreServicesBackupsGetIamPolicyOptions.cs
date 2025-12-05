using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "backups", "get-iam-policy")]
public record GcloudMetastoreServicesBackupsGetIamPolicyOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Location,
[property: CliArgument] string Service
) : GcloudOptions;