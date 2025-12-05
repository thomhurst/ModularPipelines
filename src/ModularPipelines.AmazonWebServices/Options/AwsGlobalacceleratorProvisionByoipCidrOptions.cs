using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "provision-byoip-cidr")]
public record AwsGlobalacceleratorProvisionByoipCidrOptions(
[property: CliOption("--cidr")] string Cidr,
[property: CliOption("--cidr-authorization-context")] string CidrAuthorizationContext
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}