using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "describe-effective-policy")]
public record AwsOrganizationsDescribeEffectivePolicyOptions(
[property: CommandSwitch("--policy-type")] string PolicyType
) : AwsOptions
{
    [CommandSwitch("--target-id")]
    public string? TargetId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}