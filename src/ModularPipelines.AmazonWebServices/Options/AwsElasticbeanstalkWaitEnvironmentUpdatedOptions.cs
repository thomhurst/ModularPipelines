using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "wait", "environment-updated")]
public record AwsElasticbeanstalkWaitEnvironmentUpdatedOptions : AwsOptions
{
    [CliOption("--application-name")]
    public string? ApplicationName { get; set; }

    [CliOption("--version-label")]
    public string? VersionLabel { get; set; }

    [CliOption("--environment-ids")]
    public string[]? EnvironmentIds { get; set; }

    [CliOption("--environment-names")]
    public string[]? EnvironmentNames { get; set; }

    [CliOption("--included-deleted-back-to")]
    public long? IncludedDeletedBackTo { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}