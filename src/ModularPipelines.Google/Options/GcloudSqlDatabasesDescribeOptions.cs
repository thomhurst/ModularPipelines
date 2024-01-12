using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "databases", "describe")]
public record GcloudSqlDatabasesDescribeOptions(
[property: PositionalArgument] string Database,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;