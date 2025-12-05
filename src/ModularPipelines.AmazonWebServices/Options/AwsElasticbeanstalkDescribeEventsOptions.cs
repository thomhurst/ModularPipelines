using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "describe-events")]
public record AwsElasticbeanstalkDescribeEventsOptions : AwsOptions
{
    [CliOption("--application-name")]
    public string? ApplicationName { get; set; }

    [CliOption("--version-label")]
    public string? VersionLabel { get; set; }

    [CliOption("--template-name")]
    public string? TemplateName { get; set; }

    [CliOption("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--platform-arn")]
    public string? PlatformArn { get; set; }

    [CliOption("--request-id")]
    public string? RequestId { get; set; }

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}