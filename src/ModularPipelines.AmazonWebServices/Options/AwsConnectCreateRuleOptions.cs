using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-rule")]
public record AwsConnectCreateRuleOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--trigger-event-source")] string TriggerEventSource,
[property: CommandSwitch("--function")] string Function,
[property: CommandSwitch("--actions")] string[] Actions,
[property: CommandSwitch("--publish-status")] string PublishStatus
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}