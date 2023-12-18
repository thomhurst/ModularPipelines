using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datadog", "terms", "create")]
public record AzDatadogTermsCreateOptions : AzOptions
{
    [CommandSwitch("--properties")]
    public string? Properties { get; set; }
}