using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "backups", "describe")]
public record GcloudFilestoreBackupsDescribeOptions(
[property: PositionalArgument] string Backup,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;