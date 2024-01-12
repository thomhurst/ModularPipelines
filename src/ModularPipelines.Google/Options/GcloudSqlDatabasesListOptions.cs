using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "databases", "list")]
public record GcloudSqlDatabasesListOptions(
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;