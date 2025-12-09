using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("healthcareapis", "operation-result", "show")]
public record AzHealthcareapisOperationResultShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location-name")]
    public string? LocationName { get; set; }

    [CliOption("--operation-result-id")]
    public string? OperationResultId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}