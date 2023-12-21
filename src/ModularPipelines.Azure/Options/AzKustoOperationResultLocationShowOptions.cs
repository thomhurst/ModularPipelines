using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "operation-result-location", "show")]
public record AzKustoOperationResultLocationShowOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--operation-id")]
    public string? OperationId { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}