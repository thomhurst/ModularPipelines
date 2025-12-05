using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "backups", "describe")]
public record GcloudFilestoreBackupsDescribeOptions(
[property: CliArgument] string Backup,
[property: CliOption("--region")] string Region
) : GcloudOptions;