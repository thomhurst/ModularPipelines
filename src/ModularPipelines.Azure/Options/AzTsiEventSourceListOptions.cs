using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tsi", "event-source", "list")]
public record AzTsiEventSourceListOptions(
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;