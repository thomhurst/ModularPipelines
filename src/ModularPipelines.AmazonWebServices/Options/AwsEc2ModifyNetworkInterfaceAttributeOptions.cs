using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-network-interface-attribute")]
public record AwsEc2ModifyNetworkInterfaceAttributeOptions(
[property: CliOption("--network-interface-id")] string NetworkInterfaceId
) : AwsOptions
{
    [CliOption("--attachment")]
    public string? Attachment { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--groups")]
    public string[]? Groups { get; set; }

    [CliOption("--ena-srd-specification")]
    public string? EnaSrdSpecification { get; set; }

    [CliOption("--connection-tracking-specification")]
    public string? ConnectionTrackingSpecification { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}