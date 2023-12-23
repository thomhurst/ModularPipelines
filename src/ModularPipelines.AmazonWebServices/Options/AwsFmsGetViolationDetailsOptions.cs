using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fms", "get-violation-details")]
public record AwsFmsGetViolationDetailsOptions(
[property: CommandSwitch("--policy-id")] string PolicyId,
[property: CommandSwitch("--member-account")] string MemberAccount,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}