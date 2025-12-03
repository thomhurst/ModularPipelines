using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "backups", "describe")]
public record GcloudMetastoreServicesBackupsDescribeOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Location,
[property: CliArgument] string Service
) : GcloudOptions;