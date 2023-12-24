using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "delete-subscription-filter")]
public record AwsLogsDeleteSubscriptionFilterOptions(
[property: CommandSwitch("--log-group-name")] string LogGroupName,
[property: CommandSwitch("--filter-name")] string FilterName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}