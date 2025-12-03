using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "list-apps")]
public record AwsResiliencehubListAppsOptions : AwsOptions
{
    [CliOption("--app-arn")]
    public string? AppArn { get; set; }

    [CliOption("--from-last-assessment-time")]
    public long? FromLastAssessmentTime { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--to-last-assessment-time")]
    public long? ToLastAssessmentTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}