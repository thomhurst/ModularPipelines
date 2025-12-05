using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "put-managed-insight-rules")]
public record AwsCloudwatchPutManagedInsightRulesOptions(
[property: CliOption("--managed-rules")] string[] ManagedRules
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}