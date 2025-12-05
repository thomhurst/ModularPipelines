using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "job", "pipeline", "show")]
public record AzDlaJobPipelineShowOptions(
[property: CliOption("--pipeline-identity")] string PipelineIdentity
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--end-date-time")]
    public string? EndDateTime { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--start-date-time")]
    public string? StartDateTime { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}