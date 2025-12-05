using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "service-registry", "create")]
public record AzSpringServiceRegistryCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions;