using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "model", "create")]
public record AzDtModelCreateOptions(
[property: CommandSwitch("--dt-name")] string DtName
) : AzOptions
{
    [CommandSwitch("--failure-policy")]
    public string? FailurePolicy { get; set; }

    [CommandSwitch("--fd")]
    public string? Fd { get; set; }

    [CommandSwitch("--max-models-per-batch")]
    public string? MaxModelsPerBatch { get; set; }

    [CommandSwitch("--models")]
    public string? Models { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}