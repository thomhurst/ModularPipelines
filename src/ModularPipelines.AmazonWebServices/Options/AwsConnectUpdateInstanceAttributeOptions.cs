using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-instance-attribute")]
public record AwsConnectUpdateInstanceAttributeOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--attribute-type")] string AttributeType,
[property: CommandSwitch("--value")] string Value
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}