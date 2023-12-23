using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "put-subscription-filter")]
public record AwsLogsPutSubscriptionFilterOptions(
[property: CommandSwitch("--log-group-name")] string LogGroupName,
[property: CommandSwitch("--filter-name")] string FilterName,
[property: CommandSwitch("--filter-pattern")] string FilterPattern,
[property: CommandSwitch("--destination-arn")] string DestinationArn
) : AwsOptions
{
    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--distribution")]
    public string? Distribution { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}