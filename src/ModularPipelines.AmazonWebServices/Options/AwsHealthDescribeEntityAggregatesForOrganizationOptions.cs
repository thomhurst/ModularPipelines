using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("health", "describe-entity-aggregates-for-organization")]
public record AwsHealthDescribeEntityAggregatesForOrganizationOptions(
[property: CommandSwitch("--event-arns")] string[] EventArns
) : AwsOptions
{
    [CommandSwitch("--aws-account-ids")]
    public string[]? AwsAccountIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}