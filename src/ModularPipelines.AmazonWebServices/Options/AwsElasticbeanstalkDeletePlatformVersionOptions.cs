using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "delete-platform-version")]
public record AwsElasticbeanstalkDeletePlatformVersionOptions : AwsOptions
{
    [CliOption("--platform-arn")]
    public string? PlatformArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}