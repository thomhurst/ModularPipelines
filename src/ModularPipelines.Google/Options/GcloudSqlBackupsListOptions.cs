using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "backups", "list")]
public record GcloudSqlBackupsListOptions(
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;