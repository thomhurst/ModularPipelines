using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "perimeter", "onboarded-resources", "list")]
public record AzNetworkPerimeterOnboardedResourcesListOptions(
[property: CliOption("--location")] string Location
) : AzOptions;