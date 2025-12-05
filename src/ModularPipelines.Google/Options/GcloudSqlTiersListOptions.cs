using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "tiers", "list")]
public record GcloudSqlTiersListOptions : GcloudOptions
{
    [CliFlag("--show-edition")]
    public bool? ShowEdition { get; set; }
}