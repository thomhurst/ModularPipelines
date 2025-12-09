using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops-guru", "describe-account-overview")]
public record AwsDevopsGuruDescribeAccountOverviewOptions(
[property: CliOption("--from-time")] long FromTime
) : AwsOptions
{
    [CliOption("--to-time")]
    public long? ToTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}