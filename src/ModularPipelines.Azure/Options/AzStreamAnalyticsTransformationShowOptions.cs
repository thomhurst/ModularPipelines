using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stream-analytics", "transformation", "show")]
public record AzStreamAnalyticsTransformationShowOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;