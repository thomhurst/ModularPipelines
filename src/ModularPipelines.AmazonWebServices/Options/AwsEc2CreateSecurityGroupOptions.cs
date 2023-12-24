using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-security-group")]
public record AwsEc2CreateSecurityGroupOptions(
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--group-name")] string GroupName
) : AwsOptions
{
    [CommandSwitch("--vpc-id")]
    public string? VpcId { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}