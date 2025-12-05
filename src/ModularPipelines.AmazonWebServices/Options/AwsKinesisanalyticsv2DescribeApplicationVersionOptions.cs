using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "describe-application-version")]
public record AwsKinesisanalyticsv2DescribeApplicationVersionOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--application-version-id")] long ApplicationVersionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}