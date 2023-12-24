using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("detective", "get-investigation")]
public record AwsDetectiveGetInvestigationOptions(
[property: CommandSwitch("--graph-arn")] string GraphArn,
[property: CommandSwitch("--investigation-id")] string InvestigationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}