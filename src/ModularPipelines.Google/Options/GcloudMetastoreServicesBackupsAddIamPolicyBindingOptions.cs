using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "backups", "add-iam-policy-binding")]
public record GcloudMetastoreServicesBackupsAddIamPolicyBindingOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Location,
[property: CliArgument] string Service,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;