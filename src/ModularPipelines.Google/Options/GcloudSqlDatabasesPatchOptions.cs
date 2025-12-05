using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "databases", "patch")]
public record GcloudSqlDatabasesPatchOptions(
[property: CliArgument] string Database,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliOption("--charset")]
    public string? Charset { get; set; }

    [CliOption("--collation")]
    public string? Collation { get; set; }

    [CliFlag("--diff")]
    public bool? Diff { get; set; }
}