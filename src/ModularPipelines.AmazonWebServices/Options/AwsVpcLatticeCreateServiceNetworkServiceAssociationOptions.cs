using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "create-service-network-service-association")]
public record AwsVpcLatticeCreateServiceNetworkServiceAssociationOptions(
[property: CliOption("--service-identifier")] string ServiceIdentifier,
[property: CliOption("--service-network-identifier")] string ServiceNetworkIdentifier
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}