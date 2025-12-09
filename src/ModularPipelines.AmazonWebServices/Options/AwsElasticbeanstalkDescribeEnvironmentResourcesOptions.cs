using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "describe-environment-resources")]
public record AwsElasticbeanstalkDescribeEnvironmentResourcesOptions : AwsOptions
{
    [CliOption("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}