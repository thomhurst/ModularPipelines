using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-identity-id-format")]
public record AwsEc2DescribeIdentityIdFormatOptions(
[property: CommandSwitch("--principal-arn")] string PrincipalArn
) : AwsOptions
{
    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}