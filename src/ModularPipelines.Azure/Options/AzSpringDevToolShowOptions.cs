using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "dev-tool", "show")]
public record AzSpringDevToolShowOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions;