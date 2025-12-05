using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "backups", "describe")]
public record GcloudBigtableBackupsDescribeOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Cluster,
[property: CliArgument] string Instance
) : GcloudOptions;