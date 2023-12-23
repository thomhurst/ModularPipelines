using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-event-tracker")]
public record AwsPersonalizeCreateEventTrackerOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--dataset-group-arn")] string DatasetGroupArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}