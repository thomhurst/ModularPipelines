using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-rule")]
public record AwsConnectUpdateRuleOptions(
[property: CommandSwitch("--rule-id")] string RuleId,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--function")] string Function,
[property: CommandSwitch("--actions")] string[] Actions,
[property: CommandSwitch("--publish-status")] string PublishStatus
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}