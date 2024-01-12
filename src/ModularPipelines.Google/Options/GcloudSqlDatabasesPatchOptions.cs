using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "databases", "patch")]
public record GcloudSqlDatabasesPatchOptions(
[property: PositionalArgument] string Database,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions
{
    [CommandSwitch("--charset")]
    public string? Charset { get; set; }

    [CommandSwitch("--collation")]
    public string? Collation { get; set; }

    [BooleanCommandSwitch("--diff")]
    public bool? Diff { get; set; }
}