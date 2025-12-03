using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "list-service-tags")]
public record AzNetworkListServiceTagsOptions(
[property: CliOption("--location")] string Location
) : AzOptions;