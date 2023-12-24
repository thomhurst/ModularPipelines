using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "delete-finding-aggregator")]
public record AwsSecurityhubDeleteFindingAggregatorOptions(
[property: CommandSwitch("--finding-aggregator-arn")] string FindingAggregatorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}