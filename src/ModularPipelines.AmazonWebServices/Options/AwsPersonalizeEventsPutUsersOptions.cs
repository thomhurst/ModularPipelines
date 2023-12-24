using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize-events", "put-users")]
public record AwsPersonalizeEventsPutUsersOptions(
[property: CommandSwitch("--dataset-arn")] string DatasetArn,
[property: CommandSwitch("--users")] string[] Users
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}