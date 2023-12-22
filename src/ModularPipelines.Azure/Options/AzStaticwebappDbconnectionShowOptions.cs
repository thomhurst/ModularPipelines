using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "dbconnection", "show")]
public record AzStaticwebappDbconnectionShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--detailed")]
    public bool? Detailed { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }
}