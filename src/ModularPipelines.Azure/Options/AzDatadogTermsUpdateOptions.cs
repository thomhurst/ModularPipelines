using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datadog", "terms", "update")]
public record AzDatadogTermsUpdateOptions : AzOptions
{
    [CliOption("--properties")]
    public string? Properties { get; set; }
}