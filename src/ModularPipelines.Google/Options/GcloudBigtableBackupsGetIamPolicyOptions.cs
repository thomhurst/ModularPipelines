using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "backups", "get-iam-policy")]
public record GcloudBigtableBackupsGetIamPolicyOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Instance
) : GcloudOptions;