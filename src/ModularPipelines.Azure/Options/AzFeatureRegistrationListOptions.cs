using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("feature", "registration", "list")]
public record AzFeatureRegistrationListOptions : AzOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }
}