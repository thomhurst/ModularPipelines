using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "get-finding-aggregator")]
public record AwsSecurityhubGetFindingAggregatorOptions(
[property: CommandSwitch("--finding-aggregator-arn")] string FindingAggregatorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}