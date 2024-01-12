using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "backups", "describe")]
public record GcloudSqlBackupsDescribeOptions(
[property: PositionalArgument] string Id,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;