using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-verified-access-group-policy")]
public record AwsEc2ModifyVerifiedAccessGroupPolicyOptions(
[property: CommandSwitch("--verified-access-group-id")] string VerifiedAccessGroupId
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