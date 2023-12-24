using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "remove-permission")]
public record AwsSnsRemovePermissionOptions(
[property: CommandSwitch("--topic-arn")] string TopicArn,
[property: CommandSwitch("--label")] string Label
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}