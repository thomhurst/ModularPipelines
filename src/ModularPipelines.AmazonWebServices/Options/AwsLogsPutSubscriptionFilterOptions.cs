using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-subscription-filter")]
public record AwsLogsPutSubscriptionFilterOptions(
[property: CliOption("--log-group-name")] string LogGroupName,
[property: CliOption("--filter-name")] string FilterName,
[property: CliOption("--filter-pattern")] string FilterPattern,
[property: CliOption("--destination-arn")] string DestinationArn
) : AwsOptions
{
    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--distribution")]
    public string? Distribution { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}