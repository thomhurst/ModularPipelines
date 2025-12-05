using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "backups", "describe")]
public record GcloudAlloydbBackupsDescribeOptions(
[property: CliArgument] string Backup,
[property: CliOption("--region")] string Region
) : GcloudOptions;