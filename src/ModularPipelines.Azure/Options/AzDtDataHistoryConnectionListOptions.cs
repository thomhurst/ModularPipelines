using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "data-history", "connection", "list")]
public record AzDtDataHistoryConnectionListOptions(
[property: CommandSwitch("--dt-name")] string DtName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}