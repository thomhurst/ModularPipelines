using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "get-verified-access-group-policy")]
public record AwsEc2GetVerifiedAccessGroupPolicyOptions(
[property: CommandSwitch("--verified-access-group-id")] string VerifiedAccessGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}