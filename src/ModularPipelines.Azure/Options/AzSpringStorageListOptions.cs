using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "storage", "list")]
public record AzSpringStorageListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions;