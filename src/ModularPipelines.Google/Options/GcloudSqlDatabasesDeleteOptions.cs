using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "databases", "delete")]
public record GcloudSqlDatabasesDeleteOptions(
[property: PositionalArgument] string Database,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;