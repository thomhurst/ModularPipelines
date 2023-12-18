using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "use-dev-spaces")]
public record AzAksUseDevSpacesOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--space")]
    public string? Space { get; set; }

    [BooleanCommandSwitch("--update")]
    public bool? Update { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}