using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "describe-service-updates")]
public record AwsElasticacheDescribeServiceUpdatesOptions : AwsOptions
{
    [CliOption("--service-update-name")]
    public string? ServiceUpdateName { get; set; }

    [CliOption("--service-update-status")]
    public string[]? ServiceUpdateStatus { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}