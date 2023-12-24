using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "list-service-network-service-associations")]
public record AwsVpcLatticeListServiceNetworkServiceAssociationsOptions : AwsOptions
{
    [CommandSwitch("--service-identifier")]
    public string? ServiceIdentifier { get; set; }

    [CommandSwitch("--service-network-identifier")]
    public string? ServiceNetworkIdentifier { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}