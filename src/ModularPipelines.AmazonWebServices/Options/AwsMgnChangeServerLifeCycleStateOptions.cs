using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "change-server-life-cycle-state")]
public record AwsMgnChangeServerLifeCycleStateOptions(
[property: CommandSwitch("--life-cycle")] string LifeCycle,
[property: CommandSwitch("--source-server-id")] string SourceServerId
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}