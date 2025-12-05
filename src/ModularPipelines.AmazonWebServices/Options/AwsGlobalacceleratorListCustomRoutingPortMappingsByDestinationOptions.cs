using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "list-custom-routing-port-mappings-by-destination")]
public record AwsGlobalacceleratorListCustomRoutingPortMappingsByDestinationOptions(
[property: CliOption("--endpoint-id")] string EndpointId,
[property: CliOption("--destination-address")] string DestinationAddress
) : AwsOptions
{
    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}