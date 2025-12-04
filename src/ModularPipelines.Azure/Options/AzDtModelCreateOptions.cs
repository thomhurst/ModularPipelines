using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "model", "create")]
public record AzDtModelCreateOptions(
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliOption("--failure-policy")]
    public string? FailurePolicy { get; set; }

    [CliOption("--fd")]
    public string? Fd { get; set; }

    [CliOption("--max-models-per-batch")]
    public string? MaxModelsPerBatch { get; set; }

    [CliOption("--models")]
    public string? Models { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}