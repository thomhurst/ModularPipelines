using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datadog", "terms", "create")]
public record AzDatadogTermsCreateOptions : AzOptions
{
    [CliOption("--properties")]
    public string? Properties { get; set; }
}