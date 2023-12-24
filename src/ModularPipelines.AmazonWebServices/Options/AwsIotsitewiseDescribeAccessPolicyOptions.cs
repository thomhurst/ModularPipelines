using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "describe-access-policy")]
public record AwsIotsitewiseDescribeAccessPolicyOptions(
[property: CommandSwitch("--access-policy-id")] string AccessPolicyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}