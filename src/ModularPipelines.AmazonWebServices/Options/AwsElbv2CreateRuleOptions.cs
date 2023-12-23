using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "create-rule")]
public record AwsElbv2CreateRuleOptions(
[property: CommandSwitch("--listener-arn")] string ListenerArn,
[property: CommandSwitch("--conditions")] string[] Conditions,
[property: CommandSwitch("--priority")] int Priority,
[property: CommandSwitch("--actions")] string[] Actions
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}