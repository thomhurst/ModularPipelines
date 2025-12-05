using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "job", "list")]
public record AzDlaJobListOptions : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--pipeline-id")]
    public string? PipelineId { get; set; }

    [CliOption("--recurrence-id")]
    public string? RecurrenceId { get; set; }

    [CliOption("--result")]
    public string? Result { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--submitted-after")]
    public string? SubmittedAfter { get; set; }

    [CliOption("--submitted-before")]
    public string? SubmittedBefore { get; set; }

    [CliOption("--submitter")]
    public string? Submitter { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}