using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "delete-event-tracker")]
public record AwsPersonalizeDeleteEventTrackerOptions(
[property: CommandSwitch("--event-tracker-arn")] string EventTrackerArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}