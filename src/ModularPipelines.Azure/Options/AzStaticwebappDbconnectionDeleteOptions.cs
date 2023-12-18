using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "dbconnection", "delete")]
public record AzStaticwebappDbconnectionDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

