using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "list")]
public record AzSqlServerListOptions : AzOptions
{
    [BooleanCommandSwitch("--expand-ad-admin")]
    public bool? ExpandAdAdmin { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}