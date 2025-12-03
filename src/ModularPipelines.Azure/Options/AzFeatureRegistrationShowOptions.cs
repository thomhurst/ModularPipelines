using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("feature", "registration", "show")]
public record AzFeatureRegistrationShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--provider-namespace")] string ProviderNamespace
) : AzOptions;