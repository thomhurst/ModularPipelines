using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server", "list")]
public record AzSqlServerListOptions : AzOptions
{
    [CliFlag("--expand-ad-admin")]
    public bool? ExpandAdAdmin { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}