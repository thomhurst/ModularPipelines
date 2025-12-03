using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("health", "describe-entity-aggregates-for-organization")]
public record AwsHealthDescribeEntityAggregatesForOrganizationOptions(
[property: CliOption("--event-arns")] string[] EventArns
) : AwsOptions
{
    [CliOption("--aws-account-ids")]
    public string[]? AwsAccountIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}