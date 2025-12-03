using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "list-test-grid-sessions")]
public record AwsDevicefarmListTestGridSessionsOptions(
[property: CliOption("--project-arn")] string ProjectArn
) : AwsOptions
{
    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CliOption("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CliOption("--end-time-after")]
    public long? EndTimeAfter { get; set; }

    [CliOption("--end-time-before")]
    public long? EndTimeBefore { get; set; }

    [CliOption("--max-result")]
    public int? MaxResult { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}