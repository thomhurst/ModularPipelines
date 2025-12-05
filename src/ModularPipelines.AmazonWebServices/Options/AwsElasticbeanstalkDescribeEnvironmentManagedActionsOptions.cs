using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "describe-environment-managed-actions")]
public record AwsElasticbeanstalkDescribeEnvironmentManagedActionsOptions : AwsOptions
{
    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}