using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "create-anomaly-subscription")]
public record AwsCeCreateAnomalySubscriptionOptions(
[property: CommandSwitch("--anomaly-subscription")] string AnomalySubscription
) : AwsOptions
{
    [CommandSwitch("--resource-tags")]
    public string[]? ResourceTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}