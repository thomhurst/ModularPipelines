using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "describe-platform-version")]
public record AwsElasticbeanstalkDescribePlatformVersionOptions : AwsOptions
{
    [CommandSwitch("--platform-arn")]
    public string? PlatformArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}