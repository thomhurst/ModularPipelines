using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "backups", "remove-iam-policy-binding")]
public record GcloudMetastoreServicesBackupsRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Service,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;