using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "advertise-byoip-cidr")]
public record AwsGlobalacceleratorAdvertiseByoipCidrOptions(
[property: CliOption("--cidr")] string Cidr
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}