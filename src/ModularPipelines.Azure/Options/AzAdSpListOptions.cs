using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "sp", "list")]
public record AzAdSpListOptions : AzOptions
{
    [CommandSwitch("--all")]
    public string? All { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--show-mine")]
    public string? ShowMine { get; set; }

    [CommandSwitch("--spn")]
    public string? Spn { get; set; }
}