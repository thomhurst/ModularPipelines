using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "backups", "get-iam-policy")]
public record GcloudMetastoreServicesBackupsGetIamPolicyOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Service
) : GcloudOptions;