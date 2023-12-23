using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "add-permission")]
public record AwsSnsAddPermissionOptions(
[property: CommandSwitch("--topic-arn")] string TopicArn,
[property: CommandSwitch("--label")] string Label,
[property: CommandSwitch("--aws-account-id")] string[] AwsAccountId,
[property: CommandSwitch("--action-name")] string[] ActionName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}