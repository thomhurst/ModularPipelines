using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-security-group-rules")]
public record AwsEc2ModifySecurityGroupRulesOptions(
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--security-group-rules")] string[] SecurityGroupRules
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}