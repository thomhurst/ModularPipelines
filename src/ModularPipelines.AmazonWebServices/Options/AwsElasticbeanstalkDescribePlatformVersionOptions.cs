using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "describe-platform-version")]
public record AwsElasticbeanstalkDescribePlatformVersionOptions : AwsOptions
{
    [CliOption("--platform-arn")]
    public string? PlatformArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}