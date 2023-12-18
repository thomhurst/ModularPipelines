using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datadog", "terms", "update")]
public record AzDatadogTermsUpdateOptions : AzOptions
{
    [CommandSwitch("--properties")]
    public string? Properties { get; set; }
}