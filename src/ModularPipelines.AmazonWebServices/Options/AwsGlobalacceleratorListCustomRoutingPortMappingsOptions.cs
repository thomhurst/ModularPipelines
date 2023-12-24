using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "list-custom-routing-port-mappings")]
public record AwsGlobalacceleratorListCustomRoutingPortMappingsOptions(
[property: CommandSwitch("--accelerator-arn")] string AcceleratorArn
) : AwsOptions
{
    [CommandSwitch("--endpoint-group-arn")]
    public string? EndpointGroupArn { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}