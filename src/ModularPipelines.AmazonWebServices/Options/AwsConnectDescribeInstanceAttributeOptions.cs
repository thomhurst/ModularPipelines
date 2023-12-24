using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "describe-instance-attribute")]
public record AwsConnectDescribeInstanceAttributeOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--attribute-type")] string AttributeType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}