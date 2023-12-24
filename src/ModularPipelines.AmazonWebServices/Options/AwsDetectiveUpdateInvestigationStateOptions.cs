using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("detective", "update-investigation-state")]
public record AwsDetectiveUpdateInvestigationStateOptions(
[property: CommandSwitch("--graph-arn")] string GraphArn,
[property: CommandSwitch("--investigation-id")] string InvestigationId,
[property: CommandSwitch("--state")] string State
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}