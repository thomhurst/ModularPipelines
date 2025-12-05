using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "application-accelerator", "customized-accelerator", "list")]
public record AzSpringApplicationAcceleratorCustomizedAcceleratorListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions;