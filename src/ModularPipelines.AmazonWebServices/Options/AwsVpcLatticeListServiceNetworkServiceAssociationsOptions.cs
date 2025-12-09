using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "list-service-network-service-associations")]
public record AwsVpcLatticeListServiceNetworkServiceAssociationsOptions : AwsOptions
{
    [CliOption("--service-identifier")]
    public string? ServiceIdentifier { get; set; }

    [CliOption("--service-network-identifier")]
    public string? ServiceNetworkIdentifier { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}