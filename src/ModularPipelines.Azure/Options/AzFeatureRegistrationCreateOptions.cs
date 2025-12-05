using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("feature", "registration", "create")]
public record AzFeatureRegistrationCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace")] string Namespace
) : AzOptions;