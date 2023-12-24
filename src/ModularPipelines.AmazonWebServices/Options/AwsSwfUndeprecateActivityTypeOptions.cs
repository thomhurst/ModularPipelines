using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "undeprecate-activity-type")]
public record AwsSwfUndeprecateActivityTypeOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--activity-type")] string ActivityType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}