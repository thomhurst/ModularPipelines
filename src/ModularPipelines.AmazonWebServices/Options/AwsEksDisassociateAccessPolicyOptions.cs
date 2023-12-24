using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "disassociate-access-policy")]
public record AwsEksDisassociateAccessPolicyOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--principal-arn")] string PrincipalArn,
[property: CommandSwitch("--policy-arn")] string PolicyArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}