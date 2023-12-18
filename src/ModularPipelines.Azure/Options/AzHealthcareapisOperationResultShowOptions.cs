using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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
    public string? Subscription { get; set; }
}