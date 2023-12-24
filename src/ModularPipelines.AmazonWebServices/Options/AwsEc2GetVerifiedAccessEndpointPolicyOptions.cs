using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "get-verified-access-endpoint-policy")]
public record AwsEc2GetVerifiedAccessEndpointPolicyOptions(
[property: CommandSwitch("--verified-access-endpoint-id")] string VerifiedAccessEndpointId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}