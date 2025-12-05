using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "describe-environment-managed-action-history")]
public record AwsElasticbeanstalkDescribeEnvironmentManagedActionHistoryOptions : AwsOptions
{
    [CliOption("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}