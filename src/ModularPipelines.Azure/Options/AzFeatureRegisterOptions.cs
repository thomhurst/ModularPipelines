using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("feature", "register")]
public record AzFeatureRegisterOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace")] string Namespace
) : AzOptions;