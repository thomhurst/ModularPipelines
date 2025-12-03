using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webpubsub", "list-usage")]
public record AzWebpubsubListUsageOptions(
[property: CliOption("--location")] string Location
) : AzOptions;