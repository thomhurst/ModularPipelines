using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloud-service", "list")]
public record AzCloudServiceListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;