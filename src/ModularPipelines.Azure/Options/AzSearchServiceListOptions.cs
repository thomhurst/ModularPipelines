using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("search", "service", "list")]
public record AzSearchServiceListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;