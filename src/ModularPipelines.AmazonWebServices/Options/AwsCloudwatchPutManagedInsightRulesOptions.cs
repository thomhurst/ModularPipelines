using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "put-managed-insight-rules")]
public record AwsCloudwatchPutManagedInsightRulesOptions(
[property: CommandSwitch("--managed-rules")] string[] ManagedRules
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}