using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "list-app-versions")]
public record AwsResiliencehubListAppVersionsOptions(
[property: CliOption("--app-arn")] string AppArn
) : AwsOptions
{
    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}