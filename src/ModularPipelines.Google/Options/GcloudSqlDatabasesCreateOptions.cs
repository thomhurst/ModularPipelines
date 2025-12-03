using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "databases", "create")]
public record GcloudSqlDatabasesCreateOptions(
[property: CliArgument] string Database,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--charset")]
    public string? Charset { get; set; }

    [CliOption("--collation")]
    public string? Collation { get; set; }
}