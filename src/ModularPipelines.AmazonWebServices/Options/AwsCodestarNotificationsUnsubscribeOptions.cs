using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-notifications", "unsubscribe")]
public record AwsCodestarNotificationsUnsubscribeOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--target-address")] string TargetAddress
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}