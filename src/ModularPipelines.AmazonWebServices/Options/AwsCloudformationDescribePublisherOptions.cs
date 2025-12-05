using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "describe-publisher")]
public record AwsCloudformationDescribePublisherOptions : AwsOptions
{
    [CliOption("--publisher-id")]
    public string? PublisherId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}