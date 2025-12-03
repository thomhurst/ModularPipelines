using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("feature", "registration", "list")]
public record AzFeatureRegistrationListOptions : AzOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }
}