using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "describe-application")]
public record AwsKinesisanalyticsv2DescribeApplicationOptions(
[property: CliOption("--application-name")] string ApplicationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}