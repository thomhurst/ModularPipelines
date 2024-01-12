using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "databases", "create")]
public record GcloudSqlDatabasesCreateOptions(
[property: PositionalArgument] string Database,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--charset")]
    public string? Charset { get; set; }

    [CommandSwitch("--collation")]
    public string? Collation { get; set; }
}