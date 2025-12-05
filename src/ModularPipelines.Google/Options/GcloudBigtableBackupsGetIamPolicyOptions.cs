using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "backups", "get-iam-policy")]
public record GcloudBigtableBackupsGetIamPolicyOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Cluster,
[property: CliArgument] string Instance
) : GcloudOptions;