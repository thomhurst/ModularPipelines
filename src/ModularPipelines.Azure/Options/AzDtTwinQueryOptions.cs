using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "twin", "query")]
public record AzDtTwinQueryOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--query-command")] string QueryCommand
) : AzOptions
{
    [BooleanCommandSwitch("--cost")]
    public bool? Cost { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}