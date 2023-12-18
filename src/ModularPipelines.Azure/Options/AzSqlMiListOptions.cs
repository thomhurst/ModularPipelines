using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi", "list")]
public record AzSqlMiListOptions : AzOptions
{
    [BooleanCommandSwitch("--expand-ad-admin")]
    public bool? ExpandAdAdmin { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}