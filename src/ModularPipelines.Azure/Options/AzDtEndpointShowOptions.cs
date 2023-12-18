using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "endpoint", "show")]
public record AzDtEndpointShowOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--en")] string En
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

