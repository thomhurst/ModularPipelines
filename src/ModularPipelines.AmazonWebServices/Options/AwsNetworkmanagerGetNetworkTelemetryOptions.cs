using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "get-network-telemetry")]
public record AwsNetworkmanagerGetNetworkTelemetryOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId
) : AwsOptions
{
    [CommandSwitch("--core-network-id")]
    public string? CoreNetworkId { get; set; }

    [CommandSwitch("--registered-gateway-arn")]
    public string? RegisteredGatewayArn { get; set; }

    [CommandSwitch("--aws-region")]
    public string? AwsRegion { get; set; }

    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}