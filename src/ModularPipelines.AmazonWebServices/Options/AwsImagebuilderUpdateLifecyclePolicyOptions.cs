using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "update-lifecycle-policy")]
public record AwsImagebuilderUpdateLifecyclePolicyOptions(
[property: CommandSwitch("--lifecycle-policy-arn")] string LifecyclePolicyArn,
[property: CommandSwitch("--execution-role")] string ExecutionRole,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--policy-details")] string[] PolicyDetails,
[property: CommandSwitch("--resource-selection")] string ResourceSelection
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}