using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("feature", "show")]
public record AzFeatureShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace")] string Namespace
) : AzOptions;