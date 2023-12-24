using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "detach-policy")]
public record AwsOrganizationsDetachPolicyOptions(
[property: CommandSwitch("--policy-id")] string PolicyId,
[property: CommandSwitch("--target-id")] string TargetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}