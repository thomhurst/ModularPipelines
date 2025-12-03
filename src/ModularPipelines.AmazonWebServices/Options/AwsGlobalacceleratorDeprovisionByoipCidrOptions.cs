using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "deprovision-byoip-cidr")]
public record AwsGlobalacceleratorDeprovisionByoipCidrOptions(
[property: CliOption("--cidr")] string Cidr
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}