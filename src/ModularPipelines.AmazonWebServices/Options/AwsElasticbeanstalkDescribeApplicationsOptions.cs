using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "describe-applications")]
public record AwsElasticbeanstalkDescribeApplicationsOptions : AwsOptions
{
    [CliOption("--application-names")]
    public string[]? ApplicationNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}