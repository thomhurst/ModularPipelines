using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "flags", "list")]
public record GcloudSqlFlagsListOptions : GcloudOptions
{
    [CliOption("--database-version")]
    public string? DatabaseVersion { get; set; }
}