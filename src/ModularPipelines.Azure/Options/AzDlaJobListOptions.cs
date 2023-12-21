using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "job", "list")]
public record AzDlaJobListOptions : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--pipeline-id")]
    public string? PipelineId { get; set; }

    [CommandSwitch("--recurrence-id")]
    public string? RecurrenceId { get; set; }

    [CommandSwitch("--result")]
    public string? Result { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--submitted-after")]
    public string? SubmittedAfter { get; set; }

    [CommandSwitch("--submitted-before")]
    public string? SubmittedBefore { get; set; }

    [CommandSwitch("--submitter")]
    public string? Submitter { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}