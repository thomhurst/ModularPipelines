using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudhsmv2", "create-hsm")]
public record AwsCloudhsmv2CreateHsmOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--availability-zone")] string AvailabilityZone
) : AwsOptions
{
    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}