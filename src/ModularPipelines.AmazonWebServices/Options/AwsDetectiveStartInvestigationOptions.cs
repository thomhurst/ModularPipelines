using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("detective", "start-investigation")]
public record AwsDetectiveStartInvestigationOptions(
[property: CommandSwitch("--graph-arn")] string GraphArn,
[property: CommandSwitch("--entity-arn")] string EntityArn,
[property: CommandSwitch("--scope-start-time")] long ScopeStartTime,
[property: CommandSwitch("--scope-end-time")] long ScopeEndTime
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}