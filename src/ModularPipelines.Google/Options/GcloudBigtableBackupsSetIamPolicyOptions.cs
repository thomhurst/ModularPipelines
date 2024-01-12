using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "backups", "set-iam-policy")]
public record GcloudBigtableBackupsSetIamPolicyOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;