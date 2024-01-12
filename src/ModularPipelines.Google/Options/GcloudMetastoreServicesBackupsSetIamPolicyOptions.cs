using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "backups", "set-iam-policy")]
public record GcloudMetastoreServicesBackupsSetIamPolicyOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Service,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;