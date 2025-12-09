using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "describe-trails")]
public record AwsCloudtrailDescribeTrailsOptions : AwsOptions
{
    [CliOption("--trail-name-list")]
    public string[]? TrailNameList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}