using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "backups", "describe")]
public record GcloudAlloydbBackupsDescribeOptions(
[property: PositionalArgument] string Backup,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;