using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "perimeter", "onboarded-resources", "list")]
public record AzNetworkPerimeterOnboardedResourcesListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;