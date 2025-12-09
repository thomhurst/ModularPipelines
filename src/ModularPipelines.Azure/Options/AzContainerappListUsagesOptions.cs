using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "list-usages")]
public record AzContainerappListUsagesOptions(
[property: CliOption("--location")] string Location
) : AzOptions;