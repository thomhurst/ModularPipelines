using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "get-auth-policy")]
public record AwsVpcLatticeGetAuthPolicyOptions(
[property: CommandSwitch("--resource-identifier")] string ResourceIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}