using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-security-group-rules")]
public record AwsEc2ModifySecurityGroupRulesOptions(
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--security-group-rules")] string[] SecurityGroupRules
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}