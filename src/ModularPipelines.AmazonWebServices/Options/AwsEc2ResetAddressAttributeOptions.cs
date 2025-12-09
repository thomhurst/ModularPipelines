using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "reset-address-attribute")]
public record AwsEc2ResetAddressAttributeOptions(
[property: CliOption("--allocation-id")] string AllocationId,
[property: CliOption("--attribute")] string Attribute
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}