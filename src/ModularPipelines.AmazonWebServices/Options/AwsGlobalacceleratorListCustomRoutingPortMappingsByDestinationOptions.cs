using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "list-custom-routing-port-mappings-by-destination")]
public record AwsGlobalacceleratorListCustomRoutingPortMappingsByDestinationOptions(
[property: CommandSwitch("--endpoint-id")] string EndpointId,
[property: CommandSwitch("--destination-address")] string DestinationAddress
) : AwsOptions
{
    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}