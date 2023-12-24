using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "reset-address-attribute")]
public record AwsEc2ResetAddressAttributeOptions(
[property: CommandSwitch("--allocation-id")] string AllocationId,
[property: CommandSwitch("--attribute")] string Attribute
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}