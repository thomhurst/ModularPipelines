using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "list-service-tags")]
public record AzNetworkListServiceTagsOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;