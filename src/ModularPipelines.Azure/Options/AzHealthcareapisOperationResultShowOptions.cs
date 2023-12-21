using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcareapis", "operation-result", "show")]
public record AzHealthcareapisOperationResultShowOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location-name")]
    public string? LocationName { get; set; }

    [CommandSwitch("--operation-result-id")]
    public string? OperationResultId { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}