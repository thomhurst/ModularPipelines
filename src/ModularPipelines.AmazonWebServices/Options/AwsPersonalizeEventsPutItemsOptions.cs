using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize-events", "put-items")]
public record AwsPersonalizeEventsPutItemsOptions(
[property: CommandSwitch("--dataset-arn")] string DatasetArn,
[property: CommandSwitch("--items")] string[] Items
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}