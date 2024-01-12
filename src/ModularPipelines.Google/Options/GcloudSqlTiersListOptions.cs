using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "tiers", "list")]
public record GcloudSqlTiersListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--show-edition")]
    public bool? ShowEdition { get; set; }
}