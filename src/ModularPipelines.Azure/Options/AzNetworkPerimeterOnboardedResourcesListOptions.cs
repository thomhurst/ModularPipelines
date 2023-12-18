using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "perimeter", "onboarded-resources", "list")]
public record AzNetworkPerimeterOnboardedResourcesListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}

