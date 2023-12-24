using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-verified-access-endpoint-policy")]
public record AwsEc2ModifyVerifiedAccessEndpointPolicyOptions(
[property: CommandSwitch("--verified-access-endpoint-id")] string VerifiedAccessEndpointId
) : AwsOptions
{
    [CommandSwitch("--policy-document")]
    public string? PolicyDocument { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}