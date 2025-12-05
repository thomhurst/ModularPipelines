using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("feature", "registration", "delete")]
public record AzFeatureRegistrationDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace")] string Namespace
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}