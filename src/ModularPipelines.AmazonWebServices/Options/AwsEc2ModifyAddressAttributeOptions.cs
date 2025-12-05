using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-address-attribute")]
public record AwsEc2ModifyAddressAttributeOptions(
[property: CliOption("--allocation-id")] string AllocationId
) : AwsOptions
{
    [CliOption("--domain-name")]
    public string? DomainName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}