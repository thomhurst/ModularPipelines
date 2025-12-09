using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("feature", "unregister")]
public record AzFeatureUnregisterOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace")] string Namespace
) : AzOptions;