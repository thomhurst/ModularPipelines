using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto", "operation-result", "show")]
public record AzKustoOperationResultShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--operation-id")]
    public string? OperationId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}